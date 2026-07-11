import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface EmployeeListDto {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  dateOfJoining: Date;
  status: number;
  salary: number;
  departmentName: string;
  designationTitle: string;
}

export interface ApiResponse<T> {
  data: T;
  message: string;
  isSuccess: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private http = inject(HttpClient);

  getEmployees(
    search?: string,
    departmentId?: string,
    designationId?: string,
    page: number = 1,
    pageSize: number = 10
  ): Observable<ApiResponse<EmployeeListDto[]>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    if (search) params = params.set('search', search);
    if (departmentId) params = params.set('departmentId', departmentId);
    if (designationId) params = params.set('designationId', designationId);

    return this.http.get<ApiResponse<EmployeeListDto[]>>(`${environment.apiUrl}/employees`, { params });
  }
}
