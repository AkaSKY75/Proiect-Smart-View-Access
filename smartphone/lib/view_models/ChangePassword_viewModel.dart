import 'package:flutter/material.dart';
import 'package:smartphone/view_models/base_model.dart';

class ChangePasswordView extends BaseModel {
  Future<void> changePassword(
      String currentPass, String newPass, String confirmPass,
      [BuildContext ctx]) async {
    bool result;
    result = await changePasswordService.changePass(
        currentPass, newPass, confirmPass, uid);
    if (result == false) {
      _showMyDialog(ctx, 'Logare Esuata',
          'Prola curenta/confirmarea parolei noi a fost introdusa gresit, mai incercati');
    }
    notifyListeners();
  }

  Future<void> _showMyDialog(
      BuildContext context, String title, String description) async {
    return showDialog<void>(
      context: context,
      barrierDismissible: true,
      builder: (BuildContext context) {
        return AlertDialog(
          title: Text(title),
          content: SingleChildScrollView(
            child: ListBody(
              children: <Widget>[
                Text(description),
              ],
            ),
          ),
          actions: <Widget>[
            TextButton(
              child: const Text('Ok'),
              onPressed: () {
                Navigator.of(context).pop();
              },
            ),
          ],
        );
      },
    );
  }
}
