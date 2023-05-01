# AttendanceApp - Docházková aplikace

# Úvod
Tento projekt "AttendanceApp" byl vytvořen jako součást diplomové práce na Univerzitě Tomáše Bati ve Zlíně. Cílem tohoto projektu je vytvoření aplikace pro správu docházky zaměstnanců ve firmě.

# Instalace
Pro spuštění aplikace je třeba stáhnout jeden z dostupných release balíčků. Pokud stáhnete balíček se selfcontained deployment, není potřeba instalovat žádný další software. Pokud stáhnete balíček deployment framework-dependant, je třeba mít nainstalován .NET6.0 na vašem zařízení.

Po prvním spuštění aplikace je potřeba zvolit umístění pro ukládání databáze. Apliakce při prvním spuštění vytvoří několik uživatelů pro testování, jejich klíče jsou AAA, TSE, ETR, EVA, TER. Uživatel AAA je administrátor a má přístup ke všem funkcím aplikace.

# Použití
Po spuštění aplikace se zobrazí hlavní okno, kde uživatelé zadávají svůj klíč, aby se dostali . Po přihlášení uživatelé vidí seznam předmětů, na které jsou zapsáni, a mohou zadávat svou docházku.

Administrátoři mají přístup ke všem funkcím aplikace a mohou spravovat předměty, studenty a docházku.

# Technologie
Tato aplikace byla vytvořena s použitím následujících technologií a návrhových vzorů:

.NET6

WPF

MVVM

EF

SQLite

# Autoři
Tento projekt vytvořil student Univerzity Tomáše Bati ve Zlíně jako součást své diplomové práce

Tomáš Ševců

# Licence
Tento projekt je licencován pod licencí MIT. Pro více informací si přečtěte soubor LICENSE.
