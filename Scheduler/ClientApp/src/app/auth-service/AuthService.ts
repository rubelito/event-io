import { HttpClient, HttpErrorResponse, HttpHeaders} from "@angular/common/http";
import { Inject, Injectable  } from "@angular/core";
import { Observable, Observer, of, throwError } from 'rxjs';
import { ChangePasswordResult } from "../calendar-models/changePasswordResult-Model";
import { PasswordModel } from "../calendar-models/password-Model";
import { PrivilegeModel } from "../calendar-models/privilege-model";
import { ResultModel } from "../calendar-models/resultMoodel";
import { User } from "../calendar-models/user";
import { UserBasic } from "../calendar-models/user-basic";
import { UserCredential } from '../calendar-models/user-credential';
import { UserIdentity } from "../calendar-models/user-identity";

@Injectable()
export class AuthService {
  private headers: HttpHeaders = new HttpHeaders();
  private baseUrl: string;
    
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string){
    this.headers = this.headers.set('Content-type', 'application/json');
    this.baseUrl = baseUrl;
    }

    logIn(userCredential : UserCredential): Observable<UserIdentity> {
      return this.http.post<UserIdentity>(this.baseUrl + "user/Login", userCredential, { headers: this.headers})
    }

    isLoggedIn(): boolean {
      let isLogin = localStorage.getItem("isLogin");

      if (isLogin != null && isLogin == "true"){
        return true;
      }
      return false;
   }

   isAdministrator(): boolean {
    let role = localStorage.getItem("role");

      if (role != null && role == "Administrator"){
        return true;
      }
      return false;
   }

   getCurrentUser(): Observable<User> {
    let cre = localStorage.getItem("accessToken");
    this.headers = this.headers.set('Authorization', cre!);
    return this.http.get<User>(this.baseUrl + "user/GetCurrentLogUser", { headers: this.headers});
   }

   editCurrentUser(uToEdit: User): Observable<UserBasic> {
    let cre = localStorage.getItem("accessToken");
    this.headers = this.headers.set('Authorization', cre!);
     return this.http.post<UserBasic>(this.baseUrl + "user/EditCurrentLogUser", uToEdit, { headers: this.headers});
   }

   getAllActiveUser(): Observable<UserBasic[]> {
    let cre = localStorage.getItem("accessToken");
    this.headers = this.headers.set('Authorization', cre!);
     return this.http.get<UserBasic[]>(this.baseUrl + "user/GetAllActiveUser", { headers: this.headers});
   }

   getAllUserExcludingYou(): Observable<UserBasic[]> {
    let cre = localStorage.getItem("accessToken");
    this.headers = this.headers.set('Authorization', cre!);
     return this.http.get<UserBasic[]>(this.baseUrl + "user/GetAllUserExcludingYou", { headers: this.headers});
   }

   getContacts(): Observable<UserBasic[]> {
    let cre = localStorage.getItem("accessToken");
    this.headers = this.headers.set('Authorization', cre!);
     return this.http.get<UserBasic[]>(this.baseUrl + "user/GetContacts", { headers: this.headers});
   }

   addContact(email: string): Observable<ResultModel>{
    let cre = localStorage.getItem("accessToken");
    this.headers = this.headers.set('Authorization', cre!);
     return this.http.get<ResultModel>(this.baseUrl + "user/AddContact?email=" + email, { headers: this.headers});
   }

    removeContact(contactId: number): Observable<ResultModel>{
    let cre = localStorage.getItem("accessToken");
    this.headers = this.headers.set('Authorization', cre!);
     return this.http.get<ResultModel>(this.baseUrl + "user/RemoveContact?contactId=" + contactId, { headers: this.headers});
   }

   register(model: UserBasic) : Observable<ResultModel> {
     return this.http.post<ResultModel>(this.baseUrl + "user/Register", model,{ headers: this.headers});
   }

   isUsernameExist(userName: string): Observable<boolean> {
    let cre = localStorage.getItem("accessToken");
    this.headers = this.headers.set('Authorization', cre!);
     return this.http.get<boolean>(this.baseUrl + "user/IsUserExist?userName=" + userName,{ headers: this.headers});
   }

   isEmailExist(email: string): Observable<boolean> {
    let cre = localStorage.getItem("accessToken");
    this.headers = this.headers.set('Authorization', cre!);
     return this.http.get<boolean>(this.baseUrl + "user/IsEmailExist?email=" + email, { headers: this.headers});
   }

   changePassword(model: PasswordModel): Observable<ChangePasswordResult> {
    let cre = localStorage.getItem("accessToken");
    this.headers = this.headers.set('Authorization', cre!);
     return this.http.post<ChangePasswordResult>(this.baseUrl + "user/ChangePassword", model,{ headers: this.headers});
   }

   logOut(): Observable<string> {
    localStorage.setItem("username", "");
    localStorage.setItem("isLogin", "");
    localStorage.setItem("loginStatus", "");
    localStorage.setItem('accessToken', "");
    localStorage.setItem('role', "");

    return of("logout");
   }
}