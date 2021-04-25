import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:smartphone/locator.dart';
import 'package:smartphone/models/Angajat_dataModel.dart';
import 'package:smartphone/services/Database_service.dart';

class LoginServices {
  DatabaseService _databaseService = locator<DatabaseService>();
  final FirebaseAuth _auth = FirebaseAuth.instance;
  AngajatDataModel _currentUser;
  AngajatDataModel get currentUser => _currentUser;
  Future signIn(String email, String password, [BuildContext ctx]) async {
    try {
      UserCredential result = await _auth.signInWithEmailAndPassword(
          email: email, password: password);
      User user = result.user;
      await _populateCurrentUser(user);
      return user != null;
    } catch (error) {
      var message = "Authentication failed!";
      if (error.toString().contains("invalid-email")) {
        message = "Email is not valid. Please insert a valid one!";
      } else if (error.toString().contains("wrong-password")) {
        message = "Password is not correct!";
      } else if (error.toString().contains("user-not-found")) {
        message =
            "There is no user with this email. Please register before sign in!";
      } else if (error.toString().contains("user-disabled")) {
        message = "This user was disabled!";
      } else if (error.toString().contains("too-many-requests")) {
        message = "Too many requests to sign in with this user";
      }
      return message;
    }
  }

  Future<void> _populateCurrentUser(User user) async {
    if (user != null) {
      _currentUser = await _databaseService.getUser(user.uid);
    }
  }
}
