# 🏦 Application d'Accréditation - Banque de France





https://github.com/user-attachments/assets/1473158e-7b51-4c91-aee2-27ed413b8f6a



## 📋 Description du Projet
Application web développée pour la gestion des processus d'accréditation de la Banque de France. Cette solution permet la soumission, le traitement et le suivi des demandes d'accréditation avec une interface sécurisée et des workflows automatisés.

## 🛠️ Technologies Utilisées

### Backend
- **Framework**: ASP.NET Core 6.0
- **Serveur Web**: IIS 10+
- **Base de données**: SQL Server 2019+
- **Authentification**: ASP.NET Core Identity
- **Sécurité**: JWT, HTTPS, Politiques CORS

### Frontend
- **UI Framework**: Bootstrap 5.2
- **Charting**: Chart.js
- **Icons**: Font Awesome 6.0
- **Client-side**: jQuery 3.6, AJAX

### Outils de Développement
- **IDE**: Visual Studio 2022
- **Gestion de packages**: NuGet
- **Versioning**: Git

## 📂 Structure du Projet

```
BankOfFrance-Accreditation/
│
├── 📁 Controllers/
│   ├── HomeController.cs          # Contrôleur principal et pages publiques
│   ├── AccountController.cs       # Gestion authentification et utilisateurs
│   ├── AccreditationController.cs # Traitement des demandes d'accréditation
│   ├── AdminController.cs         # Administration système
│   └── ReportController.cs        # Génération de rapports
│
├── 📁 Models/
│   ├── AccountViewModels.cs       # Modèles pour l'authentification
│   ├── AccreditationModels.cs     # Modèles métier pour les accréditations
│   ├── Entities/                  # Classes entité EF Core
│   │   ├── ApplicationUser.cs
│   │   ├── AccreditationRequest.cs
│   │   └── Document.cs
│   └── ViewModels/                # ViewModels pour les vues
│
├── 📁 Views/
│   ├── Shared/                    # Layouts et vues partielles
│   ├── Account/                   # Vues d'authentification
│   ├── Accreditation/             # Vues de gestion des accréditations
│   ├── Admin/                     # Vues d'administration
│   └── Home/                      # Vues publiques
│
├── 📁 wwwroot/
│   ├── css/                       # Feuilles de style customisées
│   ├── js/                        # Scripts JavaScript
│   ├── lib/                       # Bibliothèques externes
│   └── documents/                 # Stockage des documents uploadés
│
├── 📁 Services/
│   ├── IEmailService.cs           # Service d'envoi d'emails
│   ├── IDocumentService.cs        # Gestion des documents
│   └── IAccreditationService.cs   # Logique métier des accréditations
│
├── 📁 Data/
│   ├── ApplicationDbContext.cs    # Contexte de base de données
│   └── Migrations/                # Migrations Entity Framework
│
├── 📁 Utilities/
│   ├── CustomAttributes.cs        # Attributs customisés
│   ├── Helpers.cs                 # Classes helper
│   └── Enums.cs                   # Énumérations
│
├── web.config                     # Configuration IIS
└── Program.cs                     # Point d'entrée de l'application
```

## ⚙️ Prérequis d'Installation

### Serveur Windows
- Windows Server 2019 ou supérieur
- IIS 10+ avec les fonctionnalités ASP.NET Core
- .NET Core Hosting Bundle 6.0+
- SQL Server 2019+

### Configuration IIS
1. Installation du rôle Web Server (IIS) avec les composants:
   - .NET Extensibility 4.8
   - ASP.NET 4.8
   - ISAPI Extensions
   - ISAPI Filters

2. Installation du **.NET Core Hosting Bundle**:
   ```powershell
   # Télécharger et installer le bundle
   # Disponible sur: https://dotnet.microsoft.com/download/dotnet/6.0
   ```

3. Configuration du pool d'applications:
   - Mode pipeline: Intégré
   - .NET CLR version: Sans code managé
   - Identity: ApplicationPoolIdentity

### Dépendances .NET
Les packages NuGet requis sont définis dans le fichier `.csproj`:
```xml
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="6.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="6.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="6.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="6.0.0" />
<PackageReference Include="Dapper" Version="2.0.0" />
<PackageReference Include="ClosedXML" Version="0.95.0" />
```

## 🚀 Déploiement

### Étapes d'installation:
1. **Cloner le repository**
   ```bash
   git clone https://github.com/votre-username/BankOfFrance-Accreditation.git
   ```

2. **Configurer la base de données**
   - Exécuter les migrations Entity Framework
   ```powershell
   Update-Database
   ```

3. **Configurer IIS**
   - Créer un site pointant vers le dossier de publication
   - Configurer le pool d'applications en No Managed Code
   - Accorder les permissions au dossier pour IIS_IUSRS

4. **Configurer les paramètres**
   - Modifier `appsettings.json` avec les chaînes de connexion
   - Configurer les paramètres SMTP pour les emails

5. **Test de fonctionnement**
   - Accéder à l'URL du site
   - Vérifier que la page de login s'affiche

## 🔐 Fonctionnalités de Sécurité

- Authentification multi-facteurs
- Chiffrement des données sensibles
- Journalisation des activités (logging)
- Validation des entrées utilisateur
- Protection contre les attaques XSS et CSRF
- Gestion des rôles et permissions

## 📊 Fonctionnalités Métier

- Soumission de demandes d'accréditation
- Workflow de validation multi-niveaux
- Gestion documentaire avec versioning
- Tableaux de bord de suivi
- Génération de rapports PDF/Excel
- Notifications et alertes email

## 🗂️ Gestion des Documents

L'application intègre un système de gestion documentaire sécurisé:
- Stockage sécurisé des documents sensibles
- Contrôle de version automatique
- Historique des modifications
- Metadata et tags de classification

## 📞 Support

Pour toute question concernant le déploiement ou l'utilisation de l'application, consulter la documentation technique ou contacter l'équipe de développement.

---

*© 2024 - Application d'Accréditation Banque de France. Tous droits réservés.*
