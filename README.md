# Сервис dkp_system

# 1 - Необходимое ПО для публикации

1. ASP.NET Core Runtime 8.0.
2. В раскалдку добавить источник Nuget пакетов [PackageStorage](https://gl-1.2pp.dev/bank-finstar/libraries/package-storage)

# 2 - Файлы конфигурации для публикации

Перед запуском проекта необходимо сформировать файлы конфигурации ".env" и "appsettings.json".

### **Файл ".env"** (в CI/CD ...\_ENV, указать при запуске докер контейнера с ключом "--env-file"):

| Название | Описание                      |
|----------|-------------------------------|
| TZ       | _Часовой пояс инстанса (UTC)_ |

#### Пример содержания файла ".env"

```
TZ=UTC
```

### **Файл "appsettings.json"** - (Разместить рядом с собранным приложением):

| Название                   | Описание                                                                                                                      |
|----------------------------|-------------------------------------------------------------------------------------------------------------------------------|
| ConnectionStrings.Database | _Строка подключения к базе данных (Host=localhost;Port=5432;Database=dkp_system;Username=postgresql;Password=postgresql;)_ |
| ConnectionStrings.Rabbit   | _Строка подключения к RabbitMq (amqp://user:pass@host:10000)_                                                                 |
| ConnectionStrings.Redis    | _Строка подключения к Redis (localhost)_                                                                                      |
| ConnectionStrings.Sentry   | _Строка подключения к Sentry (https://public@sentry.example.com/1)_                                                           |

#### Пример содержания файла "appsettings.json"

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "Database": "",
        "Redis": "",
        "Rabbit": "",
        "Sentry": ""
    }
}
```

# 3 - Шаги публикации

Для публикации проекта выполните следующие шаги:

1. Откройте командную строку в корневой папке проекта где расположен файл dkp_system.sln.
2. Выполните команду "dotnet publish -c Release -o /app", чтобы собрать проекты во внутренних подпапках app.
3. Для запуска проекта выполните команду "dotnet ./dkp_system/app/dkp_system.dll".