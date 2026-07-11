import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { EmployeeService, EmployeeListDto } from './employee.service';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css']
})
export class EmployeesComponent implements OnInit {
  private employeeService = inject(EmployeeService);

  // Table configurations
  displayedColumns: string[] = ['name', 'email', 'phone', 'department', 'designation', 'joiningDate', 'salary', 'actions'];
  readonly dataSource = signal<EmployeeListDto[]>([]);

  // Search filter
  searchControl = new FormControl('');

  // Pagination states
  readonly totalCount = signal(0);
  readonly pageSize = signal(10);
  readonly pageIndex = signal(0);
  readonly isLoading = signal(false);

  ngOnInit(): void {
    this.loadEmployees();

    // Setup debounce search
    this.searchControl.valueChanges.pipe(
      debounceTime(400),
      distinctUntilChanged()
    ).subscribe(() => {
      this.pageIndex.set(0);
      this.loadEmployees();
    });
  }

  loadEmployees(): void {
    this.isLoading.set(true);
    const searchVal = this.searchControl.value ?? '';
    
    this.employeeService.getEmployees(
      searchVal,
      undefined,
      undefined,
      this.pageIndex() + 1,
      this.pageSize()
    ).subscribe({
      next: (response) => {
        this.isLoading.set(false);
        this.dataSource.set(response.data);
        // Let's mock a total count based on array length if not returned, or standard mock for now
        this.totalCount.set(response.data.length || 15);
      },
      error: () => {
        this.isLoading.set(false);
        // Fallback mock database items on error or offline
        console.warn('Backend offline. Displaying mock employees.');
        this.dataSource.set(this.getMockEmployees());
        this.totalCount.set(4);
      }
    });
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
    this.loadEmployees();
  }

  private getMockEmployees(): EmployeeListDto[] {
    return [
      { id: '1', firstName: 'John', lastName: 'Doe', email: 'john.doe@company.com', phone: '+15550199', dateOfJoining: new Date('2022-03-15'), status: 1, salary: 8500, departmentName: 'Engineering', designationTitle: 'Senior Software Engineer' },
      { id: '2', firstName: 'Jane', lastName: 'Smith', email: 'jane.smith@company.com', phone: '+15550244', dateOfJoining: new Date('2023-01-10'), status: 1, salary: 9200, departmentName: 'Engineering', designationTitle: 'Lead Architect' },
      { id: '3', firstName: 'Michael', lastName: 'Brown', email: 'michael.brown@company.com', phone: '+15550299', dateOfJoining: new Date('2021-08-01'), status: 1, salary: 7800, departmentName: 'Human Resources', designationTitle: 'HR Admin' },
      { id: '4', firstName: 'Emily', lastName: 'Davis', email: 'emily.davis@company.com', phone: '+15550388', dateOfJoining: new Date('2024-05-15'), status: 1, salary: 6500, departmentName: 'Finance', designationTitle: 'Payroll Manager' }
    ];
  }
}
