# Тема 17.1: Git — углублённое изучение

## Что ты узнаешь
- Branching стратегии: GitHub Flow, Git Flow
- Merge vs Rebase
- Cherry-pick, Stash, Tags
- .gitignore, Git LFS (для ассетов)
- Hooks

---

## Объяснение

### ЗАЧЕМ?

Git — не просто `commit` и `push`. Это инструмент **командной работы**. Без правильного branching — хаос. Без .gitignore — в репозитории мусор. Без LFS — Godot-проект весит гигабайты.

### Branching стратегии

```
GitHub Flow (простой, для большинства проектов):

main ──●────●────●────●────●──→
        \         /
         ●──●──●─
         feature/login

Правила:
1. main — всегда рабочий
2. Создай ветку из main
3. Делай коммиты в ветку
4. Открой Pull Request
5. Code Review → Merge → Удали ветку

Git Flow (для больших проектов с релизами):

main    ──●────────────────●──→  (только релизы)
           \              /
develop ────●──●──●──●──●──→     (текущая разработка)
              \     /
               ●──●              (feature/*)
```

### Merge vs Rebase

```bash
# Merge — сохраняет историю "как было"
git checkout main
git merge feature/login
# Создаёт merge commit, видна ветка в истории

# Rebase — "переносит" коммиты поверх main
git checkout feature/login
git rebase main
# История линейная, чистая

# Когда что:
# Merge: для merge в main (PR)
# Rebase: для обновления своей ветки из main

# ⚠️ НИКОГДА не rebase публичную ветку (main, develop)!
# Rebase переписывает историю!
```

### Полезные команды

```bash
# Cherry-pick — забрать конкретный коммит из другой ветки
git cherry-pick abc1234

# Stash — "отложить" текущие изменения
git stash                   # спрятать
git stash list              # список
git stash pop               # вернуть последний
git stash apply stash@{1}   # вернуть конкретный

# Tags — пометить релиз
git tag v1.0.0
git tag -a v1.0.0 -m "First release"
git push --tags

# Просмотр истории
git log --oneline --graph --all  # красивый граф
git log --since="2 weeks ago"
git blame file.cs                # кто написал каждую строку
git diff main..feature            # разница между ветками
```

### .gitignore для C# / Godot / Uno

```gitignore
# .NET
bin/
obj/
*.user
*.suo
*.vs/

# Rider
.idea/

# Godot
.godot/
*.import

# Но НЕ игнорируй:
# *.tscn, *.tres, *.gdshader — это файлы проекта!
# project.godot — это конфиг проекта!

# Uno Platform
Generated/

# Общее
*.log
.env
*.tmp
```

### Git LFS — для больших файлов

```bash
# Установка
git lfs install

# Указываем, какие файлы хранить в LFS
git lfs track "*.png"
git lfs track "*.wav"
git lfs track "*.ogg"
git lfs track "*.ttf"
git lfs track "*.fbx"
git lfs track "*.blend"

# Это создаёт файл .gitattributes:
# *.png filter=lfs diff=lfs merge=lfs -text
# *.wav filter=lfs diff=lfs merge=lfs -text

# ОБЯЗАТЕЛЬНО коммитим .gitattributes
git add .gitattributes
git commit -m "Configure Git LFS for assets"

# Теперь большие файлы хранятся на LFS-сервере,
# а в репозитории — только ссылки (~130 байт)

# Зачем?
# Без LFS: Godot-проект с ассетами = 2 GB репозиторий
# С LFS:   Godot-проект = 50 MB репозиторий + ассеты на сервере
```

### Git Hooks — автоматизация

```bash
# .git/hooks/pre-commit — запускается ПЕРЕД коммитом
#!/bin/sh
dotnet build
if [ $? -ne 0 ]; then
    echo "Build failed! Fix errors before committing."
    exit 1
fi

# .git/hooks/commit-msg — проверяет сообщение коммита
#!/bin/sh
if ! grep -qE "^(feat|fix|docs|style|refactor|test|chore):" "$1"; then
    echo "Commit message must start with: feat:, fix:, docs:, etc."
    exit 1
fi
```

### Conventional Commits

```
feat: добавить систему инвентаря
fix: исправить крит-урон при нулевой броне
docs: обновить README
refactor: выделить DamageCalculator в отдельный класс
test: добавить тесты для QuestSystem
chore: обновить зависимости NuGet
style: форматирование кода

Формат:
<тип>(<область>): <описание>

feat(inventory): добавить стакинг предметов
fix(combat): исправить расчёт урона при баффах
```

---

## Мини-упражнения

1. **⭐** Создай репозиторий, ветку, сделай изменения, merge через PR на GitHub.
2. **⭐** Настрой .gitignore для Godot-проекта. Проверь что .godot/ не попадает в git.
3. **⭐⭐** Настрой Git LFS для ассетов (png, wav, ogg).

## Что дальше
Дальше — **GitHub Actions** (17.2).
