# AetherInstall — Design System Documentation

## 🎨 Brand Identity

**App Name:** AetherInstall  
**Tagline:** Your Apps. One Breath. Instant.  
**Theme:** AetherGlow Pro (Dark Mode with Acrylic)

---

## 🌈 AetherGlow Pro Color System

### Primary Palette
```xml
Background:      #FF0A0A14  (Deep Space Black)
Surface:         #FF14141F  (Dark Charcoal)
Card:            #26FFFFFF  (15% White - Glass)
CardHover:       #40FFFFFF  (25% White - Glass Hover)
Border:          #33FFFFFF  (20% White - Glass Border)
```

### Accent Palette
```xml
Accent:          #FF00E6FF  (Cyan Bright)
AccentHover:     #FF33EBFF  (Cyan Lighter)
AccentPressed:   #FF00C7E6  (Cyan Darker)
SecondaryAccent: #FF7B61FF  (Purple Soft)
Success:         #FF00FF9D  (Green Neon)
Warning:         #FFFFD166  (Orange Warm)
Error:           #FFFF6B6B  (Red Soft)
```

### Text Palette
```xml
TextPrimary:     #FFFFFFFF  (Pure White)
TextSecondary:   #CCFFFFFF  (80% White)
TextTertiary:    #99FFFFFF  (60% White)
TextDisabled:    #66FFFFFF  (40% White)
OnAccent:        #FF0A0A14  (Dark on Bright)
```

---

## ✨ Visual Effects

### Glow Effects (Drop Shadows)
```xml
GlowPrimary:     DropShadow Color=#FF00E6FF Opacity=0.75 BlurRadius=28
GlowSecondary:   DropShadow Color=#FF7B61FF Opacity=0.6 BlurRadius=20
GlowSuccess:     DropShadow Color=#FF00FF9D Opacity=0.75 BlurRadius=28
```

### Acrylic Backdrop
- TransparencyLevelHint: "AcrylicBlur"
- Background opacity: 0.8
- Creates frosted glass effect

---

## 🎭 UI Components

### Window Configuration
```xml
Title: "AetherInstall — One-Breath App Setup"
Width: 740px
Height: 580px
CanResize: False
WindowStartupLocation: CenterScreen
ExtendClientAreaToDecorationsHint: True (Custom title bar)
```

### Header Section (100px height)
- **Logo:** Animated circular icon with cyan accent + glow
- **Title:** "AetherInstall" (30px, Bold, White)
- **Tagline:** "One-Breath App Setup" (13px, Italic, Tertiary)
- **Settings Button:** Icon-only, glass hover effect

### Button Styles

#### Primary Button (Call to Action)
- Background: Cyan Accent (#FF00E6FF)
- Foreground: Dark (#FF0A0A14)
- CornerRadius: 22px (Fully rounded)
- Height: 44-48px
- FontWeight: SemiBold
- Effect: Primary Glow
- Hover: Lighter cyan
- Pressed: Darker cyan

#### Secondary Button (Supporting Action)
- Background: Glass Card (#26FFFFFF)
- Foreground: White
- CornerRadius: 22px
- Height: 44px
- FontWeight: Medium
- Hover: Brighter glass (#40FFFFFF)

#### Icon Button (Utility)
- Background: Transparent
- Size: 40x40px
- CornerRadius: 20px (Circular)
- Hover: Glass card overlay

### Card Components
- Background: Glass Card (#26FFFFFF)
- CornerRadius: 16-18px
- Padding: 18px
- Hover State: Brighter glass
- Transition: 0.2s smooth

### App Selection Cards
- Layout: WrapPanel (responsive grid)
- MinWidth: 280px
- Spacing: 16px
- Components:
  - Checkbox (left, 24x24px, cyan accent)
  - App Name (16px, Medium weight)
  - App Description (12px, tertiary color)
  - Icon Badge (right, 36x36px circle)

### Progress Indicators
- Height: 10px
- CornerRadius: 5px
- Foreground: Cyan Accent
- Background: Glass Card
- Effect: Primary Glow
- IsIndeterminate: True during loading

---

## 📄 Page Flow

### Page 0: Welcome
- **Layout:** Center-aligned, vertical stack
- **Title:** "Welcome to AetherInstall" (34px, Bold)
- **Description:** Tagline + features (16px, Secondary)
- **Pro Tip Card:** Glass card with secondary glow, italic text
- **Action:** "Get Started →" (Primary button)

### Page 1: App Selection
- **Layout:** Grid with header + scrollable content
- **Title:** "Select Your Apps" (26px, SemiBold)
- **Subtitle:** Installation info (14px, Secondary)
- **App Grid:** WrapPanel of glass cards with checkboxes
- **Counter:** Dynamic "Install X Apps" button text
- **Actions:** "Back" (Secondary) + "Install X Apps" (Primary)

### Page 2: Progress
- **Layout:** Center-aligned, vertical stack
- **Title:** "Installing Your Universe..." (30px, Bold)
- **Progress Bar:** 520px width with glow
- **Status Text:** Current operation (15px, Secondary)
- **Log Box:** Glass card container with scrollable logs (520px, max 140px height)
- **Note:** No action buttons during installation

### Page 3: Finish
- **Layout:** Center-aligned, vertical stack
- **Icon:** Large success checkmark (88x88px) with green glow
- **Title:** "All Systems Go!" (34px, Bold)
- **Message:** Dynamic success summary (16px, Secondary)
- **Action:** "Close" (Secondary button, 110x48px)

---

## 🎯 Typography Hierarchy

### Font Family
Primary: Inter (embedded or system)  
Fallback: Segoe UI, Arial

### Scale
```
Display:    34px  Bold        (Page titles)
Heading 1:  30px  Bold        (Section headers)
Heading 2:  26px  SemiBold    (Subsections)
Body Large: 16px  Medium      (Primary content)
Body:       15px  Regular     (Standard text)
Body Small: 14px  Regular     (Supporting text)
Caption:    13px  Italic      (Hints, tips)
Tiny:       12px  Regular     (Metadata)
```

---

## ♿ Accessibility

### Contrast Ratios
- Primary text on dark: 21:1 (AAA)
- Secondary text on dark: 12:1 (AAA)
- Tertiary text on dark: 7:1 (AA)
- Accent on dark: 13:1 (AAA)
- Dark text on accent: 14:1 (AAA)

### Keyboard Navigation
- All buttons are keyboard accessible
- Tab order: Logo/Settings → Primary Action → Secondary Action
- Enter key: Trigger primary action
- Escape key: Cancel/Back (if applicable)

### Screen Readers
- Semantic HTML structure
- ARIA labels on icon-only buttons
- Status announcements during installation

---

## 🎬 Animations & Transitions

### Page Transitions
- Type: CrossFade
- Duration: 0.35s
- Easing: Ease-in-out

### Button Interactions
- Hover: Background color transition (0.2s)
- Press: Instant state change
- Release: Return to hover state

### Card Hover
- Background: Glass brightness increase
- Transition: 0.2s smooth

### Progress Bar
- Indeterminate: Animated pulse
- Determinate: Smooth value interpolation

---

## 🚀 Unique Features

1. **Professional AetherGlow Color System** - 5-tier accent palette
2. **Acrylic Glass Morphism** - Semi-transparent cards with backdrop blur
3. **Dynamic Glow Effects** - Cyan accent shadows on interactive elements
4. **Smooth Cross-Fade Transitions** - Page changes with 0.35s fade
5. **Typography Hierarchy** - Inter font with proper weight scales
6. **Micro-Interactions** - Hover states, button press feedback
7. **Pro Tip Callouts** - Secondary accent glow on hint cards
8. **Accessible Contrast** - AA/AAA compliant ratios
9. **Modern Rounded Geometry** - 18-22px corner radius
10. **Responsive Card Grid** - WrapPanel adapts to content

---

## 📦 Implementation Files

### Core Files
```
Assets/Styles.axaml              - AetherGlow Pro theme definitions
Views/MainWindow.axaml           - Main UI layout and structure
ViewModels/MainWindowViewModel.cs - Navigation logic and state
App.axaml                        - Dark mode configuration
```

### Key Resources
- **Brushes:** 18 color definitions
- **Effects:** 3 drop shadow configurations
- **Styles:** 10+ component styles (Button, Card, CheckBox, etc.)
- **Pages:** 4 wizard steps (Welcome, Selection, Progress, Finish)

---

## 🎨 Brand Assets

### Logo
- SVG path-based circular icon
- Size: 40x40px in header
- Color: Cyan Accent (#FF00E6FF)
- Effect: Primary Glow (28px blur)

### Color Usage Guidelines
- **Accent (Cyan):** Primary actions, focus states, progress indicators
- **Secondary (Purple):** Pro tips, decorative elements, secondary highlights
- **Success (Green):** Completion states, success icons, positive feedback
- **Warning (Orange):** In-progress states, caution indicators
- **Error (Red):** Failed states, error messages, destructive actions

---

## 📐 Grid System

### Margins & Padding
```
Container Margins:  36px (all sides)
Section Padding:    32px
Card Padding:       18-20px
Button Padding:     24px horizontal, 12px vertical
Spacing (Stack):    16-36px between elements
```

### Responsive Breakpoints
- Fixed window: 740x580px
- No responsive breakpoints (desktop-first design)
- WrapPanel cards: Auto-flow based on MinWidth (280px)

---

*Design System v1.0 — AetherInstall*  
*Last Updated: October 30, 2025*
