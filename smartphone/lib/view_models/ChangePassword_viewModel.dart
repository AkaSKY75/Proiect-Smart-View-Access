import 'package:flutter/material.dart';
import 'package:smartphone/view_models/base_model.dart';

class ChangePasswordView extends BaseModel {
  Future<void> changePassword(
      String currentPass, String newPass, String confirmPass) async {
   
     await changePasswordService.changePass(currentPass, newPass, confirmPass, uid);
  

    notifyListeners();
  }
}
