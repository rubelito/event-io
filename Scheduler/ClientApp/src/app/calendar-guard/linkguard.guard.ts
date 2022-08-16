import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class LinkguardGuard implements CanActivate {

  constructor(private router: Router){ }

  canActivate(route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
      let passThrough = false;

      if (localStorage.getItem('isLogin') == 'true'){
        passThrough = true;
      }
      else {
        this.router.navigateByUrl("/");
      }
      return passThrough;
  }
}
