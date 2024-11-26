# CinemaManager 🎥

CinemaManager to prosta aplikacja konsolowa stworzona na wzór architektury MVC. Głównym celem projektu jest zarządzanie kinem za pomocą trzech paneli, umożliwiających obsługę bazy danych filmów, sal kinowych i pracowników. Projekt został stworzony jako część zajęć "Komunikacja Człowiek-Komputer" na Politechnice Białostockiej.

---

## Funkcjonalności 🛠️

### Logowanie
- Logowanie do aplikacji za pomocą anonimowego kodu przypisanego użytkownikowi.

### Zarządzanie filmami
- **Dodawanie filmów**: wprowadzanie nowych filmów do bazy danych.
- **Usuwanie filmów**: usuwanie filmów na podstawie tytułu.
- **Przegląd filmów**: podgląd pełnej bazy danych filmów.

### Zarządzanie pracownikami
- **Dodawanie pracowników**: dodawanie nowych pracowników do systemu.
- **Dodawanie podwładnych**: przypisywanie pracowników do przełożonych.
- **Modyfikowanie danych**: edycja informacji o pracownikach.
- **Przegląd pracowników**: wyświetlanie listy wszystkich pracowników.

### Zarządzanie salami i repertuarem
- **Podgląd sal**: przegląd informacji o sali kinowej wraz z listą zaplanowanych filmów.
- **Planowanie filmów**: przypisywanie filmów do konkretnych sal kinowych.
- **Ustawianie czasu rozpoczęcia**: ciekawy motyw graficzny do definiowania godzin rozpoczęcia seansów.

### Inne
- **Zapis i odczyt danych w formacie JSON**: trwała persystencja danych aplikacji.
- **Wyświetlanie zawartości baz danych**: intuicyjne prezentowanie danych.

---

## Wykorzystane technologie 📚

- **C#**: język programowania.
- **Spectre.Console**: biblioteka służąca do tworzenia atrakcyjnych wizualnie interfejsów konsolowych.
- **JSON**: format używany do zapisu i odczytu danych.

---

## Jak uruchomić? 🚀

1. **Klonowanie repozytorium**:
   ```bash
   git clone https://github.com/your-username/CinemaManager.git
   cd CinemaManager
2. **Budowanie projektu** (Upewnij się że masz zainstalowane .NET SDK):
   ```bash
   dotnet build
3. **Uruchomienie**:
   ```bash
   dotnet run

## Autor ✨
Projekt wykonany przez studenta Politechniki Białostockiej na zajęcia "Komunikacja Człowiek-Komputer".
