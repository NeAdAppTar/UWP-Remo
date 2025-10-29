using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UWPCleaner
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> uwpApps = new()
        {
            { "Solitaire Collection", "*solitaire*" },
            { "Сообщения", "*messaging*" },
            { "Кортана", "*cortana*" },
            { "Тех. поддержка", "*gethelp*" },
            { "Центр отзывов", "*feedback*" },
            { "Dev Home", "*devhome*" },
            { "3D Builder", "*3dbuilder*" },
            { "OneNote", "*onenote*" },
            { "Карты", "*maps*" },
            { "Clipchamp", "*clipchamp*" },
            { "Люди (People)", "*people*" },
            { "Почта и календарь", "*communicationsapps*" },
            { "Get Started", "*getstarted*" },
            { "Skype", "*skypeapp*" },
            { "Записки (Sticky Notes)", "*sticky*" },
            { "Новости", "*bingnews*" },
            { "Погода", "*bingweather*" },
            { "Звукозапись", "*soundrecorder*" },
            { "Ваш телефон", "*YourPhone*" },
            { "Камера ⚠️ ОПАСНО: Не будут работать видеозвонки в TG, Zoom", "*WindowsCamera*" },
            { "Медиаплеер", "*MediaPlayer*" },
            { "Фильмы и ТВ", "*ZuneVideo*" },
            { "Xbox ⚠️ ОПАСНО: Игры от XBOX перестанут работать", "*Xbox*" },
            { "Microsoft To Do", "*Todos*" }

        };

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await RefreshInstalledAsync();
        }

        private async void refreshButton_Click(object sender, EventArgs e)
        {
            await RefreshInstalledAsync();
        }

        private async void removeButton_Click(object sender, EventArgs e)
        {
            await RemoveSelectedAsync();
        }

        private async Task RefreshInstalledAsync()
        {
            removeButton.Enabled = false;
            refreshButton.Enabled = false;
            progressBar.Value = 0;
            listBox.Items.Clear();
            logBox.Clear();
            AppendLog("Проверка установленных UWP-приложений...\r\n");

            int total = uwpApps.Count;
            int step = 100 / total;
            progressBar.Value = 0;

            foreach (var app in uwpApps)
            {
                bool installed = await IsAppInstalledAsync(app.Value);
                string display = $"{app.Key} — {(installed ? "🟢 Установлено" : "⚪ Нет")}";

                listBox.Items.Add(display, installed);
                AppendLog($"{display}");
                progressBar.Value = Math.Min(progressBar.Value + step, 100);
            }

            AppendLog("\r\nПроверка завершена.");
            removeButton.Enabled = true;
            refreshButton.Enabled = true;
        }

        private async Task<bool> IsAppInstalledAsync(string pattern)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-NoLogo -NoProfile -Command \"if (Get-AppxPackage {pattern}) {{ exit 0 }} else {{ exit 1 }}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    using var process = Process.Start(psi);
                    process.WaitForExit();
                    return process.ExitCode == 0;
                }
                catch
                {
                    return false;
                }
            });
        }

        private async Task RemoveSelectedAsync()
        {
            removeButton.Enabled = false;
            refreshButton.Enabled = false;
            progressBar.Value = 0;

            var selectedApps = new List<(string Name, string Pattern)>();

            foreach (var item in listBox.CheckedItems)
            {
                string text = item.ToString();
                foreach (var kvp in uwpApps)
                {
                    if (text.StartsWith(kvp.Key))
                        selectedApps.Add((kvp.Key, kvp.Value));
                }
            }

            if (selectedApps.Count == 0)
            {
                MessageBox.Show("Нет выбранных установленных приложений для удаления.", "Инфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
                removeButton.Enabled = true;
                refreshButton.Enabled = true;
                return;
            }

            int step = 100 / selectedApps.Count;

            foreach (var app in selectedApps)
            {
                AppendLog($"\r\n[{DateTime.Now:T}] Удаляю {app.Name}...");
                string cmd = $"Get-AppxPackage {app.Pattern} | Remove-AppxPackage; " +
                             $"Get-AppxProvisionedPackage -Online | where DisplayName -like '{app.Pattern}' | Remove-AppxProvisionedPackage -Online";

                bool ok = await RunPowerShellAsync(cmd);
                if (ok)
                    AppendLog($"[{DateTime.Now:T}] ✔ {app.Name} удалено.");
                else
                    AppendLog($"[{DateTime.Now:T}] ⚠ Не удалось удалить {app.Name}.");

                progressBar.Value = Math.Min(progressBar.Value + step, 100);
            }

            AppendLog("\r\nУдаление завершено. Обновляю список...");
            await RefreshInstalledAsync();
        }

        private async Task<bool> RunPowerShellAsync(string command)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-NoLogo -NoProfile -ExecutionPolicy Bypass -Command \"{command}\"",
                        Verb = "runas",
                        UseShellExecute = true,
                        CreateNoWindow = true
                    };
                    using var process = Process.Start(psi);
                    process.WaitForExit();
                    return process.ExitCode == 0;
                }
                catch (Exception ex)
                {
                    AppendLog($"Ошибка: {ex.Message}");
                    return false;
                }
            });
        }

        private void AppendLog(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendLog), text);
                return;
            }
            logBox.AppendText(text + Environment.NewLine);
        }
    }
}
