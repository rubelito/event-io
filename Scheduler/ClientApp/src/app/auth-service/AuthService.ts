import { HttpClient } from "@angular/common/http";
import { createApiParams } from "../http/api-params";
import { Inject, Injectable  } from "@angular/core";
import { clearStoredSession } from "../http/auth-session";
import { Observable, of } from 'rxjs';
import { ChangePasswordResult } from "../calendar-models/changePasswordResult-Model";
import { PasswordModel } from "../calendar-models/password-Model";
import { ResultModel } from "../calendar-models/resultMoodel";
import { User } from "../calendar-models/user";
import { UserBasic } from "../calendar-models/user-basic";
import { UserCredential } from '../calendar-models/user-credential';
import { UserIdentity } from "../calendar-models/user-identity";

@Injectable()
export class AuthService {

  private baseUrl: string;
    
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string){
    this.baseUrl = baseUrl;
    }

    logIn(userCredential : UserCredential): Observable<UserIdentity> {
      return this.http.post<UserIdentity>(this.baseUrl + "user/Login", userCredential)
    }

    isLoggedIn(): boolean {
      let isLogin = localStorage.getItem("isLogin");

      if (isLogin != null && isLogin == "true"){
        return true;
      }
      return false;
   }

   getLoggedUserName(): string {
    var username = localStorage.getItem("username");

      if (username != null && username != ""){
        return username;
      }
      return "";
   }

   getLoggedUserId(): number {
    var userId = localStorage.getItem("id");

    if (userId != "" && userId != null){
      return +userId;
    }
    else {
      return 0;
    }
   }

   isAdministrator(): boolean {
    let role = localStorage.getItem("role");

      if (role != null && role == "Administrator"){
        return true;
      }
      return false;
   }

   getCurrentUser(): Observable<User> {
    return this.http.get<User>(this.baseUrl + "user/GetCurrentLogUser");
   }

   editCurrentUser(uToEdit: User): Observable<UserBasic> {
     return this.http.post<UserBasic>(this.baseUrl + "user/EditCurrentLogUser", uToEdit);
   }

   getAllActiveUser(): Observable<UserBasic[]> {
     return this.http.get<UserBasic[]>(this.baseUrl + "user/GetAllActiveUser");
   }

   getAllUserExcludingYou(): Observable<UserBasic[]> {
     return this.http.get<UserBasic[]>(this.baseUrl + "user/GetAllUserExcludingYou");
   }

   getContacts(): Observable<UserBasic[]> {
     return this.http.get<UserBasic[]>(this.baseUrl + "user/GetContacts");
   }

   addContact(email: string): Observable<ResultModel>{
     return this.http.get<ResultModel>(this.baseUrl + "user/AddContact", { params: createApiParams().set("email", email) });
   }

    removeContact(contactId: number): Observable<ResultModel>{
     return this.http.get<ResultModel>(this.baseUrl + "user/RemoveContact", { params: createApiParams().set("contactId", contactId) });
   }

   register(model: UserBasic) : Observable<ResultModel> {
     return this.http.post<ResultModel>(this.baseUrl + "user/Register", model);
   }

   isUsernameExist(userName: string): Observable<boolean> {
     return this.http.get<boolean>(this.baseUrl + "user/IsUserExist", { params: createApiParams().set("userName", userName) });
   }

   isEmailExist(email: string): Observable<boolean> {
     return this.http.get<boolean>(this.baseUrl + "user/IsEmailExist", { params: createApiParams().set("email", email) });
   }

   changePassword(model: PasswordModel): Observable<ChangePasswordResult> {
     return this.http.post<ChangePasswordResult>(this.baseUrl + "user/ChangePassword", model);
   }

   uploadProfileImage(file: FormData): Observable<ResultModel> {
    // Let the browser supply the multipart boundary.
    return this.http.post<ResultModel>(this.baseUrl + "user/UploadProfilePicture", file);
   }

   removeProfilePicture(userId: number): Observable<ResultModel> {
    return this.http.get<ResultModel>(this.baseUrl + "user/RemoveProfilePicture", {
      params: createApiParams().set("userId", userId)
    });
   }

   getAvatar(userId: number): Observable<Blob> {
    return this.http.post(this.baseUrl + "user/GetAvatar", userId, { responseType: 'blob' });
   }

   logOut(): Observable<string> {
    clearStoredSession();
    return of("logout");
   }
}
