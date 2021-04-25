import 'package:smartphone/view_models/base_model.dart';

import 'package:flutter/material.dart';
import 'base_model.dart';
import '../locator.dart';

class LoginView extends BaseModel {
  Future login(String email, String password, GlobalKey<FormState> _form,
      [BuildContext ctx]) async {
    var isValid = true;
    var result;

    if (isValid) {
      setBusy(true);
      result = await loginServices.signIn(email, password, ctx);
      print(result);
      setBusy(false);
    }
    if (result != null) {
      if (result is bool) {
        if (result) {
          Navigator.pushNamed(ctx, "/profile");
        } else
          print("Login Esuat");
      } else {
        print("ESUAT DIN FIREBASE");
      }
    }
  }
}
