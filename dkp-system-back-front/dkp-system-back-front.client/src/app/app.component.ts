import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import config from '../config.json'; // Импортируем JSON

interface Player {
  id?: number;
  name: string;
  surname: string;
  characterName: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  // Поля для формы добавления
  firstName: string = '';
  lastName: string = '';
  characterName: string = '';

  // Поле для поиска
  searchCharacterName: string = '';

  // Строка, куда будем выводить результат
  result: string = '';

  // URL из config.json
  private apiBaseUrl = config.baseUrl;

  constructor(private http: HttpClient) { }

  /** Добавить игрока */
  onAddPlayer() {
    const newPlayer: Player = {
      name: this.firstName.trim(),
      surname: this.lastName.trim(),
      characterName: this.characterName.trim(),
    };

    // Делаем POST-запрос через HttpClient
    this.http.post(`${this.apiBaseUrl}/Players/Add`, newPlayer, { responseType: 'text' })
      .subscribe({
        next: () => {
          this.result = '<div class="alert alert-success">Игрок успешно добавлен!</div>';
          // Очищаем поля формы
          this.firstName = '';
          this.lastName = '';
          this.characterName = '';
        },
        error: (err) => {
          this.result = `<div class="alert alert-danger">Ошибка при добавлении игрока: ${err.message}</div>`;
        }
      });
  }

  /** Найти игрока по имени персонажа */
  onSearchPlayer() {
    const searchValue = this.searchCharacterName.trim();
    if (!searchValue) {
      this.result = '<div class="alert alert-warning">Введите имя персонажа для поиска.</div>';
      return;
    }

    this.http.get<Player | null>(`${this.apiBaseUrl}/Players/Get?characterName=${encodeURIComponent(searchValue)}`)
      .subscribe({
        next: (player) => {
          if (player) {
            this.result = `
              <div class="card">
                <div class="card-body">
                  <h5 class="card-title">${player.name} ${player.surname}</h5>
                  <p class="card-text">Персонаж: ${player.characterName}</p>
                </div>
              </div>
            `;
          } else {
            this.result = '<div class="alert alert-info">Игрок не найден.</div>';
          }
        },
        error: (err) => {
          this.result = `<div class="alert alert-danger">Ошибка при поиске игрока: ${err.message}</div>`;
        }
      });
  }

  /** Получить список всех игроков */
  onGetAllPlayers() {
    this.http.get<Player[]>(`${this.apiBaseUrl}/Players/GetAll`)
      .subscribe({
        next: (players) => {
          if (!players.length) {
            this.result = '<div class="alert alert-info">Игроки не найдены.</div>';
            return;
          }
          let html = '<h3>Список игроков:</h3><ul class="list-group">';
          players.forEach((player) => {
            html += `<li class="list-group-item">
              <strong>${player.name} ${player.surname}</strong> - Персонаж: ${player.characterName}
            </li>`;
          });
          html += '</ul>';
          this.result = html;
        },
        error: (err) => {
          this.result = `<div class="alert alert-danger">Ошибка при получении списка игроков: ${err.message}</div>`;
        }
      });
  }
}
