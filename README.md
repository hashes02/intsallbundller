# ✨ AetherInstall — One-Breath App Setup

<div align="center">

![AetherInstall](https://img.shields.io/badge/AetherInstall-v1.0-00E6FF?style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![Avalonia](https://img.shields.io/badge/Avalonia-11.1.3-7B61FF?style=for-the-badge)
![License](https://img.shields.io/badge/License-MIT-00FF9D?style=for-the-badge)

**Your Apps. One Breath. Instant.**

*A stunning, professional application installer with AetherGlow Pro dark theme*

[Features](#-features) • [Installation](#-installation) • [Usage](#-usage) • [Screenshots](#-screenshots) • [Development](#-development)

</div>

---

## 🌌 Overview

**AetherInstall** is a next-generation application installer built with **Avalonia UI** and **.NET 8**, featuring the exclusive **AetherGlow Pro** color system. Install your favorite apps in one breath with a beautiful glass-morphism UI, professional acrylic effects, and smooth animations.

### Why AetherInstall?

- ✨ **AetherGlow Pro Theme** - Professional dark theme with 5-tier accent states and dynamic glow effects
- 🎨 **Glass Morphism UI** - Acrylic blur, card hover states, and smooth cross-fade transitions
- 🌍 **Cross-Platform** - Works on Windows, macOS, and Linux (via Avalonia)
- ⚡ **Fast & Lightweight** - Single-file executable (~117MB)
- 🔒 **Secure** - SHA256 verification and automatic latest version resolution
- 🎯 **Smart Detection** - Detects already-installed applications
- 🚫 **No Bloatware** - Clean, silent installations without bundled offers
- ♿ **Accessible** - AA/AAA compliant contrast ratios

---

## ✨ Features

### Core Functionality
- **One-Breath Installation** - Install multiple applications simultaneously with one click
- **Auto-Update Resolution** - Automatically fetches latest official versions
- **Silent Installation** - No popups or user intervention required
- **Smart Detection** - Detects and skips already-installed software
- **Real-Time Progress** - Live status updates with beautiful progress indicators
- **Error Handling** - Comprehensive retry logic with fallback URLs

### UI/UX Excellence
- **AetherGlow Pro Colors** - Cyan accent (#00E6FF) with dynamic hover states
- **Glass Cards** - Semi-transparent cards with backdrop blur
- **Glow Effects** - Dynamic drop shadows on interactive elements
- **Typography Hierarchy** - Inter font family with proper weight scales
- **Micro-Interactions** - Smooth transitions on hover, press, and page changes
- **Responsive Grid** - Wrap panel layout for flexible app card display

### Supported Applications

| Application | Version | Silent Install |
|------------|---------|----------------|
| 🌐 Google Chrome | Latest Stable | ✅ |
| 🎬 VLC Media Player | Latest | ✅ |
| 📊 Microsoft 365 Apps | Latest | ✅ |
| 📦 7-Zip | Latest | ✅ |
| 🖥️ RustDesk | Latest | ✅ |
| 📹 Zoom | Latest | ✅ |

### UI/UX Features
- **Modern Card-Based Layout** - Clean, organized interface
- **Search Functionality** - Quick filtering of applications
- **Responsive Design** - Adapts to different window sizes
- **Hover Effects** - Smooth transitions and visual feedback
- **Custom Styling** - Tailwind-inspired color palette and typography

---

## 🎨 Screenshots

### Main Interface
```
┌─────────────────────────────────────────────────┐
│  AppBundle                                      │
│  Install essential applications with one click  │
│                                                 │
│  🔍 Search applications...                      │
│                                                 │
│  Available Applications                         │
│  ┌────────────────────────────────────────┐   │
│  │ ☑ Google Chrome                        │   │
│  │ ☑ VLC Media Player                     │   │
│  │ ☐ Microsoft 365 Apps                   │   │
│  │ ☐ 7-Zip                                │   │
│  │ ☐ RustDesk                             │   │
│  │ ☐ Zoom                                 │   │
│  └────────────────────────────────────────┘   │
│                                                 │
│  [ Install 2 applications ]                     │
└─────────────────────────────────────────────────┘
```

---

## 📥 Installation

### Prerequisites
- **Windows 10/11** (64-bit) - Primary platform
- **macOS** (via Avalonia) - Planned support
- **Linux** (via Avalonia) - Planned support
- **.NET 8 Runtime** - Not required (self-contained executable)

### Download & Run

#### Option 1: Download Pre-built Binary
1. Download the latest `AppBundle.exe` from [Releases](../../releases)
2. Run the executable (requires admin rights for installation)
3. Select applications to install
4. Click "Install" and wait for completion

#### Option 2: Build from Source
```bash
# Clone the repository
git clone https://github.com/hashes02/intsallbundller.git
cd intsallbundller

# Build the project
dotnet build -c Release

# Run the application
dotnet run -c Release
```

#### Option 3: Publish Single-File Executable
```bash
# Windows
.\publish.bat

# Linux/macOS
./publish.sh
```

The executable will be located at:
```
bin/Release/net8.0/win-x64/publish/AppBundle.exe
```

---

## 🚀 Usage

### Basic Usage

1. **Launch AppBundle**
   ```bash
   AppBundle.exe
   ```

2. **Select Applications**
   - Check the boxes next to applications you want to install
   - Use the search box to filter applications

3. **Install**
   - Click the "Install" button
   - Grant administrator permission when prompted
   - Wait for installations to complete

### Advanced Features

#### Search Applications
Type in the search box to filter applications by name:
```
Search: "chrome" → Shows only Google Chrome
```

#### Installation Status
Each application shows its current status:
- 🔄 **Downloading...** - Fetching installer
- ⚙️ **Installing...** - Running installation
- ✅ **Installed successfully** - Completed
- ❌ **Failed** - Error occurred
- ⚠️ **Already installed** - Skipped

---

## 🛠️ Development

### Technology Stack

- **Framework**: [Avalonia UI 11.1.3](https://avaloniaui.net/)
- **Runtime**: .NET 8.0
- **Architecture**: MVVM (Model-View-ViewModel)
- **UI Library**: CommunityToolkit.Mvvm
- **Styling**: Custom XAML (Tailwind CSS-inspired)

### Project Structure

```
AppBundle/
├── Assets/
│   └── Styles.axaml          # Custom UI styles
├── Services/
│   └── InstallationService.cs # Installation logic
├── ViewModels/
│   ├── ViewModelBase.cs      # Base ViewModel
│   ├── MainWindowViewModel.cs # Main UI logic
│   └── AppItemViewModel.cs   # App item logic
├── Views/
│   └── MainWindow.axaml      # Main window UI
├── AppResolver.cs            # Download URL resolver
├── AppSource.cs              # Data models
├── apps.json                 # Application definitions
├── App.axaml                 # Application setup
└── Program.cs                # Entry point
```

### Building

#### Debug Build
```bash
dotnet build -c Debug
dotnet run
```

#### Release Build
```bash
dotnet build -c Release
```

#### Publish Single-File
```bash
dotnet publish -c Release -r win-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:PublishReadyToRun=true \
  -p:PublishTrimmed=false
```

### Adding New Applications

Edit `apps.json`:
```json
{
  "apps": [
    {
      "id": "myapp",
      "name": "My Application",
      "source": "direct",
      "url": "https://example.com/myapp-installer.exe",
      "args": "/S",
      "detect": "HKLM\\SOFTWARE\\MyApp"
    }
  ]
}
```

**Properties:**
- `id`: Unique identifier
- `name`: Display name
- `source`: `direct`, `omaha`, or `videolan`
- `url`: Download URL (for direct sources)
- `args`: Silent installation arguments
- `detect`: Registry path for detection

---

## 🎨 Design System

### Color Palette (Tailwind-Inspired)

```css
/* Primary Colors */
--primary-blue:    #3B82F6;  /* Blue-500 */
--primary-hover:   #2563EB;  /* Blue-600 */
--primary-pressed: #1D4ED8;  /* Blue-700 */

/* Status Colors */
--success-green:   #22C55E;  /* Green-500 */
--warning-amber:   #F59E0B;  /* Amber-500 */
--error-red:       #EF4444;  /* Red-500 */

/* Neutral Colors */
--gray-900:        #111827;  /* Text Primary */
--gray-500:        #6B7280;  /* Text Secondary */
--gray-200:        #E5E7EB;  /* Borders */
--gray-50:         #F9FAFB;  /* Background */
```

### Typography

```css
/* Headings */
--title-size:      32px;     /* Bold, Gray-900 */
--subtitle-size:   16px;     /* Regular, Gray-500 */
--heading-size:    18px;     /* SemiBold, Gray-900 */

/* Body Text */
--app-name-size:   16px;     /* SemiBold, Gray-900 */
--app-status-size: 14px;     /* Regular, Status Colors */
--button-size:     16px;     /* SemiBold, White */
```

### Spacing System

```css
--space-xs:  8px;
--space-sm:  12px;
--space-md:  16px;
--space-lg:  24px;
--space-xl:  32px;
```

---

## 🔧 Configuration

### App Manifest
Located at `app.manifest`:
- **Execution Level**: `asInvoker` (request admin when needed)
- **DPI Awareness**: PerMonitorV2
- **Windows Compatibility**: Windows 10/11

### Project Configuration
Located at `AppBundle.csproj`:
- **Output Type**: WinExe (GUI application)
- **Target Framework**: net8.0
- **Self-Contained**: true
- **Single File**: true
- **Ready to Run**: true

---

## 🤝 Contributing

Contributions are welcome! Please follow these guidelines:

1. **Fork the repository**
2. **Create a feature branch** (`git checkout -b feature/amazing-feature`)
3. **Commit your changes** (`git commit -m 'Add amazing feature'`)
4. **Push to the branch** (`git push origin feature/amazing-feature`)
5. **Open a Pull Request**

### Code Style
- Follow C# naming conventions
- Use meaningful variable names
- Add XML documentation for public APIs
- Keep methods focused and short
- Write XAML comments for complex UI

---

## 📝 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- **[Avalonia UI](https://avaloniaui.net/)** - Cross-platform UI framework
- **[Ninite](https://ninite.com/)** - Original inspiration
- **[Tailwind CSS](https://tailwindcss.com/)** - Design system inspiration
- **[CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)** - MVVM helpers

---

## 📞 Support

- **Issues**: [GitHub Issues](../../issues)
- **Discussions**: [GitHub Discussions](../../discussions)
- **Email**: support@appbundle.example.com

---

## 🗺️ Roadmap

### Version 1.1 (Planned)
- [ ] macOS support
- [ ] Linux support
- [ ] Custom app list import/export
- [ ] Installation profiles
- [ ] Dark theme

### Version 1.2 (Planned)
- [ ] Update checker for installed apps
- [ ] Uninstall functionality
- [ ] Portable application support
- [ ] Silent mode (command-line)
- [ ] Scheduled installations

### Version 2.0 (Future)
- [ ] Cloud sync for settings
- [ ] Enterprise deployment features
- [ ] Custom branding support
- [ ] Plugin system
- [ ] Multi-language support

---

<div align="center">

**Made with ❤️ using Avalonia UI and .NET 8**

[⬆ Back to Top](#-appbundle---modern-application-installer)

</div>
