**Auteur :** Anicet Mizumbi — projet réalisé au Cégep de Granby (TP4, cours de développement d'applications WPF avec MVVM)

> Ce README est écrit pour un **développeur débutant**. Il explique comment lancer le projet, comment il est construit, et comment le **refaire toi-même étape par étape**.

---

## 📑 Sommaire

1. [Ce que fait l'application](#1--ce-que-fait-lapplication)
2. [Technologies utilisées](#2--technologies-utilisées)
3. [Lancer le projet sur ton ordinateur](#3--lancer-le-projet-sur-ton-ordinateur)
4. [Structure des dossiers](#4--structure-des-dossiers)
5. [Le modèle de données](#5--le-modèle-de-données)
6. [Le patron MVVM expliqué simplement](#6--le-patron-mvvm-expliqué-simplement)
7. [Les règles d'affaires](#7--les-règles-daffaires)
8. [Refaire le projet seul, étape par étape](#8--refaire-le-projet-seul-étape-par-étape)
9. [Erreurs fréquentes](#9--erreurs-fréquentes)
10. [Idées pour aller plus loin](#10--idées-pour-aller-plus-loin)

---

## 1. 🏋️ Ce que fait l'application

Une application de bureau pour gérer un **centre sportif** : ses **cours**, ses **membres** et leurs **inscriptions**. Les données sont enregistrées dans une base **SQL Server**.

| Écran | Ce qu'on y fait |
|---|---|
| **Fenêtre principale** | Voir les cours à venir et leurs places disponibles ; filtrer par type de cours avec la liste déroulante |
| **Gestion → Cours** | Créer, modifier et supprimer des cours |
| **Détails d'un cours** | Formulaire avec validation : nom, coût, dates, nombre de places, type |
| **Gestion → Membres** | Créer, modifier et supprimer des membres |
| **Détails d'un membre** | Formulaire avec validation : prénom, nom, date de naissance ; bouton **Inscription** |
| **Inscriptions d'un membre** | Inscrire le membre à un cours ou le désinscrire |

Quand on ferme la fenêtre de gestion des cours, la liste de la fenêtre principale se **met à jour automatiquement**.

---

## 2. 🧰 Technologies utilisées

| Outil | À quoi il sert |
|---|---|
| **C#** / **.NET 8** | Le langage et la plateforme |
| **WPF** + **XAML** | L'interface de bureau Windows |
| **MVVM** | L'architecture : séparer l'affichage (View) de la logique (ViewModel) et des données (Model) |
| **Entity Framework Core 8** | L'ORM : il transforme les classes C# en tables SQL et écrit les requêtes à ta place |
| **SQL Server Express** | La base de données |
| **LINQ** | Les requêtes en C# (`Where`, `Include`, `Any`, `Count`…) |

> 💡 **ORM (Object-Relational Mapping)** : au lieu d'écrire `SELECT * FROM Cours WHERE ...`, tu écris `context.Cours.Where(...).ToList()`. Entity Framework traduit en SQL pour toi.

---

## 3. ▶️ Lancer le projet sur ton ordinateur

### Prérequis

- **Windows**
- **Visual Studio 2022** avec la charge de travail **« Développement .NET Desktop »**
- **SQL Server Express** (gratuit) et, idéalement, **SQL Server Management Studio (SSMS)** pour voir les tables

### Étape 1 — Adapter la chaîne de connexion

Ouvre `TP4/Models/CentreSportifDbContext.cs`. La chaîne de connexion pointe vers **mon** ordinateur (`ANICETPC\SQLEXPRESS`). Remplace ce nom par celui de **ton** serveur :

```csharp
optionsBuilder.UseSqlServer(
    "Data Source=.\\SQLEXPRESS;Initial Catalog=DevLogTP4;Trusted_Connection=true;TrustServerCertificate=true;");
```

> `.` veut dire « cet ordinateur ». `Trusted_Connection=true` utilise ta session Windows : **aucun mot de passe** n'est écrit dans le code.

### Étape 2 — Créer la base de données

Dans Visual Studio : **Outils** → **Gestionnaire de package NuGet** → **Console du Gestionnaire de package**, puis :

```powershell
Update-Database
```

Entity Framework lit le dossier `Migrations/` et crée la base `DevLogTP4` avec ses 4 tables.

### Étape 3 — Ajouter des types de cours

La base est créée **vide**. Ajoute quelques types de cours dans SSMS (sinon la liste déroulante des types sera vide) :

```sql
INSERT INTO TypeCours (Nom) VALUES ('Natation'), ('Yoga'), ('Musculation'), ('Soccer');
```

### Étape 4 — Lancer

Ouvre **`TP4.sln`** et appuie sur **F5**. 🎉

---

## 4. 📁 Structure des dossiers

```
TP4/
├── App.xaml                 ← Point de départ : ouvre MainWindow
├── MainWindow.xaml          ← Fenêtre principale (liste des cours + filtre)
├── RelayCommand.cs          ← Classe utilitaire qui transforme une méthode en ICommand
├── Models/                  ← Les DONNÉES (M de MVVM)
│   ├── Cours.cs
│   ├── Membre.cs
│   ├── Inscription.cs
│   ├── TypeCours.cs
│   └── CentreSportifDbContext.cs   ← Le lien avec la base de données
├── Services/                ← L'ACCÈS AUX DONNÉES : toutes les requêtes Entity Framework
│   ├── CoursService.cs
│   ├── MembreService.cs
│   └── InscriptionService.cs
├── ViewModels/              ← La LOGIQUE des écrans (VM de MVVM)
│   ├── MainWindowVM.cs
│   ├── GestionDesCoursVM.cs / DetailsCoursVM.cs
│   ├── GestionMembresVM.cs  / DetailsMembreVM.cs
│   └── DetailsInscriptionVM.cs
├── Views/                   ← Les ÉCRANS en XAML (V de MVVM)
│   ├── GestionCoursView.xaml / DetailsCoursView.xaml
│   ├── GestionMembres.xaml   / DetailsMembreView.xaml
│   └── DetailsInscriptionView.xaml
├── Events/                  ← Les EventArgs : les messages échangés entre ViewModels
└── Migrations/              ← Générées par Entity Framework : l'historique du schéma de la base
```

Le chemin d'une donnée est toujours le même :

```
View (XAML)  ⇄  ViewModel  →  Service  →  DbContext  →  SQL Server
  binding        logique       requêtes     Entity
                                            Framework
```

---

## 5. 🗃️ Le modèle de données

```
TypeCours 1 ──── * Cours 1 ──── * Inscription * ──── 1 Membre
 (Yoga…)          (Yoga débutant)    (date)              (Marie Tremblay)
```

| Table | Champs principaux | Relation |
|---|---|---|
| `TypeCours` | `TypeCoursId`, `Nom` | Un type regroupe plusieurs cours |
| `Cours` | `CoursId`, `Nom`, `Cout`, `DateDebut`, `DateFin`, `NbPlace`, `TypeCoursId` | Un cours a un type et plusieurs inscriptions |
| `Membre` | `MembreId`, `Prenom`, `Nom`, `DateNaissance` | Un membre a plusieurs inscriptions |
| `Inscription` | `InscriptionId`, `DateInscrit`, `MembreId`, `CoursId` | La table de liaison entre un membre et un cours |

`Inscription` est une **table de liaison** : un membre peut suivre plusieurs cours, et un cours a plusieurs membres. C'est une relation **plusieurs-à-plusieurs**.

La propriété `PlaceDisponible` de `Cours` est **calculée** (`NbPlace - Inscriptions.Count()`). L'attribut `[NotMapped]` dit à Entity Framework de **ne pas** en faire une colonne.

---

## 6. 🧠 Le patron MVVM expliqué simplement

**MVVM = Model – View – ViewModel.** L'idée : la View (le XAML) ne contient **aucune logique**. Elle se **lie** (*binding*) aux propriétés et aux commandes du ViewModel.

```
┌──────────────┐   Binding (données)    ┌────────────────┐   appelle   ┌──────────┐
│     VIEW     │ ◄────────────────────► │   VIEWMODEL    │ ──────────► │ SERVICE  │
│   (XAML)     │   Command (actions)    │ (logique, C#)  │             │ + MODEL  │
└──────────────┘ ─────────────────────► └────────────────┘             └──────────┘
   Pas de logique        INotifyPropertyChanged prévient la View d'un changement
```

Le lien se fait **une seule fois** dans le code-behind, avec le `DataContext` :

```csharp
public MainWindow()
{
    InitializeComponent();
    this.DataContext = new MainWindowVM();
}
```

### 6.1 Le binding de données

Dans le XAML, `{Binding NomDeLaPropriété}` relie un contrôle à une propriété du ViewModel :

```xml
<ComboBox ItemsSource="{Binding TypeCours}" DisplayMemberPath="Nom" SelectedItem="{Binding Type}"/>
<DataGrid ItemsSource="{Binding ListCours}" .../>
```

Quand l'utilisateur choisit un type, la propriété `Type` du ViewModel change, et son `set` appelle `filtrer()`. **Aucun événement `SelectionChanged` à écrire !**

### 6.2 `INotifyPropertyChanged` : prévenir la View

Si le ViewModel change une propriété, la View doit être **prévenue** pour se redessiner :

```csharp
public ObservableCollection<Cours> ListCours
{
    get => listCours;
    set
    {
        listCours = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListCours)));
    }
}
```

👉 Règle : une propriété que le **ViewModel** modifie lui-même doit déclencher `PropertyChanged`. Une propriété modifiée seulement par l'utilisateur, via le binding, n'en a pas besoin.

### 6.3 Les commandes : `ICommand` et `RelayCommand`

En MVVM, un bouton n'a pas de `Click`. Il a une **commande** :

```xml
<Button Content="Sauvegarder" Command="{Binding Sauvegarder}"/>
```

```csharp
public ICommand Sauvegarder => sauvegarder ??= new RelayCommand(SaveCours, CanSaveCours);

private bool CanSaveCours(object? obj) => !errorList.Values.Any(e => !string.IsNullOrEmpty(e));
private void SaveCours(object? obj) { /* appelle le service */ }
```

- `SaveCours` = **ce que fait** le bouton.
- `CanSaveCours` = **si le bouton est actif**. Quand il renvoie `false`, WPF **grise le bouton automatiquement**. C'est comme ça que « Sauvegarder » reste désactivé tant que le formulaire contient une erreur.

`RelayCommand` est une petite classe réutilisable qui transforme ces deux méthodes en `ICommand`.

### 6.4 La validation avec `IDataErrorInfo`

Les ViewModels de détails implémentent `IDataErrorInfo`. WPF appelle l'indexeur `this[string propertyName]` à chaque frappe et affiche une **bordure rouge** si le message n'est pas vide :

```csharp
public string this[string propertyName]
{
    get
    {
        if (propertyName == nameof(Nom) && string.IsNullOrEmpty(Nom))
            return "LE NOM NE PEUT PAS ETRE VIDE";
        return "";
    }
}
```

```xml
<TextBox Text="{Binding Nom, UpdateSourceTrigger=PropertyChanged, ValidatesOnDataErrors=True}"/>
```

- `ValidatesOnDataErrors=True` active la validation.
- `UpdateSourceTrigger=PropertyChanged` valide **à chaque lettre** (sinon, seulement quand on quitte le champ).
- Un `Style` dans `Window.Resources` affiche le message d'erreur en **info-bulle**.

### 6.5 Faire communiquer deux ViewModels

Quand on sauvegarde un cours dans la fenêtre de détails, la liste de la fenêtre de gestion doit se mettre à jour. Le `DetailsCoursVM` **lance un événement**, et le `GestionDesCoursVM` **s'y abonne** avant d'ouvrir la fenêtre :

```csharp
DetailsCoursVM vm = new DetailsCoursVM();
vm.sauCoEvent += (s, e) =>
{
    if (e.Cours != null) ListCours.Add(e.Cours);
};
new DetailsCoursView(vm).ShowDialog();
```

Les classes du dossier `Events/` (`SauvegardeCoEvent`, `RemoveEvent`…) transportent l'objet concerné.

### 6.6 Les services et Entity Framework

Chaque service ouvre un `DbContext` le temps d'une opération, grâce à `using` :

```csharp
public List<Cours> GetCoursByType(TypeCours type)
{
    using (CentreSportifDbContext context = new CentreSportifDbContext())
    {
        return context.Cours
            .Include(c => c.TypeCours)      // charge aussi le type (jointure)
            .Include(c => c.Inscriptions)   // charge aussi les inscriptions
            .Where(c => c.TypeCoursId == type.TypeCoursId)
            .ToList();                      // exécute la requête SQL
    }
}
```

> 💡 Sans `Include`, `cours.TypeCours` serait `null` : Entity Framework ne charge **pas** les tables liées par défaut.

---

## 7. 📏 Les règles d'affaires

| Règle | Où elle est codée |
|---|---|
| Nom, prénom, dates, nombre de places et type sont obligatoires | `IDataErrorInfo` dans `DetailsCoursVM` et `DetailsMembreVM` |
| Le coût d'un cours doit être un nombre **pair** et non nul | `DetailsCoursVM` |
| La date de début doit être **avant** la date de fin | `DetailsCoursVM` |
| La date de naissance doit être dans le **passé** | `DetailsMembreVM` |
| Le bouton Inscription est **caché** pour un membre pas encore sauvegardé | `DetailsMembreVM` (`PeutSinscrire`) |
| Un membre ne peut pas s'inscrire **deux fois** au même cours | `InscriptionService.AddInscription` |
| Un membre ne peut pas suivre **plus de 3 cours en même temps** (périodes qui se chevauchent) | `InscriptionService.AddInscription` |
| On ne peut pas se désinscrire d'un cours **déjà commencé** | `DetailsInscriptionVM.CanRemoveInscription` |
| Supprimer un cours ou un membre supprime aussi ses inscriptions | `CoursService` / `MembreService` (`RemoveRange`) |
| La fenêtre principale n'affiche que les cours **à venir** | `CoursService.GetCoursAll` |

Comment savoir si deux périodes se **chevauchent** ? Deux cours A et B se chevauchent si A commence avant la fin de B **et** B commence avant la fin de A :

```csharp
i.Cours.DateDebut <= cours.DateFin && cours.DateDebut <= i.Cours.DateFin
```

---

## 8. 🛠️ Refaire le projet seul, étape par étape

Teste avec **F5** après chaque étape.

### Étape 1 — Créer le projet et installer Entity Framework

Crée une **Application WPF** (.NET 8), puis installe ces paquets NuGet : `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Tools` et `Microsoft.EntityFrameworkCore.Design`.

### Étape 2 — Les modèles

Crée le dossier `Models/` avec `TypeCours`, `Cours`, `Membre` et `Inscription` (section 5), puis le `DbContext` avec un `DbSet<>` par table et la chaîne de connexion.

### Étape 3 — La base de données

```powershell
Add-Migration initial
Update-Database
```

✅ **Test** : dans SSMS, la base et ses 4 tables existent.

### Étape 4 — `RelayCommand`

Copie la classe `RelayCommand` : elle servira à toutes tes commandes.

### Étape 5 — Le premier écran MVVM

Crée `MainWindowVM` avec une `ObservableCollection<Cours> ListCours`, puis fais `DataContext = new MainWindowVM()` dans `MainWindow`. Affiche la liste dans un `DataGrid` avec `{Binding ListCours}`.

✅ **Test** : les cours ajoutés à la main dans SSMS s'affichent.

### Étape 6 — Les services

Déplace toutes les requêtes dans `CoursService`, `MembreService` et `InscriptionService`. Un ViewModel ne doit **jamais** créer de `DbContext` lui-même.

### Étape 7 — La gestion des cours

Une fenêtre avec un `DataGrid` et 3 boutons liés aux commandes **Créer**, **Modifier** et **Supprimer**. Le `CanExecute` de Modifier et Supprimer renvoie `false` tant qu'aucun cours n'est sélectionné.

### Étape 8 — Le formulaire avec validation

Crée `DetailsCoursVM` avec `IDataErrorInfo` (section 6.4) et lie chaque champ avec `ValidatesOnDataErrors=True`.

✅ **Test** : laisse le nom vide ; la bordure devient rouge et « Sauvegarder » se grise.

### Étape 9 — La communication entre ViewModels

Ajoute l'événement `sauCoEvent` (section 6.5) pour mettre à jour la liste après une sauvegarde.

### Étape 10 — Les membres et les inscriptions

Refais les étapes 7 à 9 pour les membres, puis ajoute l'écran d'inscription avec ses règles (section 7).

### Étape 11 — Le filtre par type

Dans `MainWindowVM`, ajoute un faux type « Tous » (Id = 0) en tête de liste, et appelle `filtrer()` dans le `set` de la propriété `Type`.

> 🎯 **Défi** : ajoute une recherche de membre par nom, sans écrire un seul événement `Click`.

---

## 9. 🐞 Erreurs fréquentes

| Problème | Cause probable | Solution |
|---|---|---|
| `SqlException : A network-related or instance-specific error…` | Le nom du serveur dans la chaîne de connexion est faux | Mets `.\\SQLEXPRESS` ou le nom de ton serveur (section 3) |
| `Update-Database` n'est pas reconnu | Le paquet `Microsoft.EntityFrameworkCore.Tools` manque | Installe-le avec NuGet |
| La liste déroulante des types est vide | La table `TypeCours` est vide | Ajoute des types (section 3, étape 3) |
| La View ne se met pas à jour | `PropertyChanged` n'est pas déclenché, ou la liste est une `List` | Déclenche `PropertyChanged` et utilise une `ObservableCollection` |
| Un binding ne fonctionne pas, sans aucune erreur | Faute de frappe dans le nom de la propriété | Regarde la fenêtre **Sortie** de Visual Studio : les erreurs de binding y sont écrites |
| `cours.TypeCours` vaut `null` | Le `Include` manque dans la requête | Ajoute `.Include(c => c.TypeCours)` |
| Le bouton reste gris | Le `CanExecute` renvoie `false` | Mets un point d'arrêt dans la méthode `Can...` |

---

## 10. 💡 Idées pour aller plus loin

- [ ] Ajouter des **données de départ** (types de cours) avec `HasData` dans le `DbContext`, pour ne plus avoir à les insérer à la main
- [ ] Déplacer la chaîne de connexion dans un fichier de configuration au lieu du code
- [ ] Le sélecteur de date de naissance commence en 2025 (`DisplayDateStart`) : le retirer pour pouvoir choisir des dates plus anciennes
- [ ] Afficher les dates sans l'heure dans les `DataGrid` (`StringFormat=d`)
- [ ] Donner de vrais titres aux fenêtres (aujourd'hui « TP4 », « DetailsCoursView »…)
- [ ] Renommer le namespace `Semaine10Exercices` de `RelayCommand` en `TP4`
- [ ] Rendre les requêtes **asynchrones** (`ToListAsync`, `SaveChangesAsync`) pour ne pas figer l'interface
- [ ] Utiliser l'**injection de dépendances** pour fournir les services aux ViewModels

---

## 👤 Auteur

**Anicet Mizumbi** — [github.com/anicetmiz](https://github.com/anicetmiz)
