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
          _showMyDialog(ctx, 'Logare Esuata',
              'Emailul/Parola utilizat pentru logare nu exista sau este introdusa gresit');
      } else {
        print("ESUAT DIN FIREBASE");
      }
    }
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
