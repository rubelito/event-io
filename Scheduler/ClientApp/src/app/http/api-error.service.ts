import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MessageboxComponent } from '../calendar-components/messagebox/messagebox.component';

@Injectable({ providedIn: 'root' })
export class ApiErrorService {
  private readonly visibleMessages = new Set<string>();

  constructor(private dialog: MatDialog) {}

  notify(error: HttpErrorResponse): void {
    const message = this.getMessage(error);
    // Concurrent requests can fail together; show each message only once.
    if (this.visibleMessages.has(message)) {
      return;
    }
    this.visibleMessages.add(message);
    const ref = this.dialog.open(MessageboxComponent, {
      width: '400px',
      data: { title: 'Request failed', message, hasNoCancel: true, icon: 'warning' }
    });
    ref.afterClosed().subscribe(() => this.visibleMessages.delete(message));
  }

  private getMessage(error: HttpErrorResponse): string {
    switch (error.status) {
      case 0: return 'Unable to reach the server. Check your connection and try again.';
      case 401: return 'Your session is no longer valid. Please log in again.';
      case 403: return 'You do not have permission to perform this action.';
    }
    if (error.status >= 500) {
      return 'The server could not complete the request. Please try again later.';
    }
    const body = error.error;
    if (body && typeof body === 'object') {
      // Support ASP.NET validation responses and the existing API message shape.
      if (body.errors && typeof body.errors === 'object') {
        const messages = Object.values(body.errors)
          .reduce<string[]>((all, value) => all.concat(Array.isArray(value) ? value : []), [])
          .filter((value): value is string => typeof value === 'string');
        if (messages.length) {
          return messages.join('\n');
        }
      }
      const message = body.detail || body.Message || body.message || body.title;
      if (typeof message === 'string' && message.trim()) {
        return message;
      }
    }
    return 'The request could not be completed. Please try again.';
  }
}
