import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-feature-placeholder',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule],
  template: `
    <div class="placeholder-wrapper">
      <mat-card class="placeholder-card">
        <mat-card-content class="placeholder-content">
          <mat-icon class="placeholder-icon">construction</mat-icon>
          <h2>{{ featureName() }} Module</h2>
          <p>This module is under construction and will be integrated into the dashboard layout soon.</p>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .placeholder-wrapper {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: 400px;
    }
    .placeholder-card {
      width: 100%;
      max-width: 500px;
      text-align: center;
      border-radius: 12px;
      border: 1px solid #e2e8f0;
      box-shadow: 0 1px 3px rgba(0,0,0,0.05);
      background-color: #ffffff;
      padding: 32px 16px;
    }
    .placeholder-content {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 16px;
    }
    .placeholder-icon {
      font-size: 64px;
      width: 64px;
      height: 64px;
      color: #0284c7;
    }
    h2 {
      font-size: 20px;
      font-weight: 700;
      color: #0f172a;
      margin: 0;
    }
    p {
      font-size: 14px;
      color: #64748b;
      margin: 0;
      line-height: 1.5;
    }
    :host-context(.dark-theme) .placeholder-card {
      background-color: #1e293b;
      border: 1px solid #334155;
    }
    :host-context(.dark-theme) h2 {
      color: #f8fafc;
    }
    :host-context(.dark-theme) p {
      color: #cbd5e1;
    }
  `]
})
export class FeaturePlaceholderComponent implements OnInit {
  private route = inject(ActivatedRoute);
  readonly featureName = signal('Feature');

  ngOnInit(): void {
    // Determine feature name based on path
    const path = this.route.snapshot.url[0]?.path || 'Feature';
    const formatted = path.charAt(0).toUpperCase() + path.slice(1);
    this.featureName.set(formatted);
  }
}
