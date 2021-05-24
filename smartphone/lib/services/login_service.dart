import 'dart:convert';

import 'package:cloud_firestore/cloud_firestore.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:smartphone/locator.dart';
import 'package:smartphone/models/Angajat_dataModel.dart';
import 'package:smartphone/services/Database_service.dart';
import 'package:crypto/crypto.dart';

class LoginServices {
  DatabaseService _databaseService = locator<DatabaseService>();
  FirebaseFirestore _firestore = FirebaseFirestore.instance;
  AngajatDataModel _currentUser;
  AngajatDataModel get currentUser => _currentUser;
  Future<bool> signIn(String email, String password, [BuildContext ctx]) async {
    bool flag = false;
    try {
      QueryDocumentSnapshot data1;
      QuerySnapshot response;
      Query queryCred;
      CollectionReference refCertification = _firestore.collection("Angajat");
      List<int> passwordEncode = utf8.encode(password);
      Digest hash = sha256.convert(passwordEncode);
      queryCred = refCertification.limit(20);
      response = await queryCred.get();
      if (response.docs.isNotEmpty) {
        for (data1 in response.docs) {
          try {
            if ((data1.data()['email'] == email ||
                    data1.data()['email_firma'] == email) &&
                data1.data()['parola'] == hash.toString()) {
              await _populateCurrentUser(data1.id);
              flag = true;
            }
          } catch (error) {
            print("$error");
          }
        }
      }
    } catch (error) {
      print("$error");
    }
    return flag;
  }

  Future<void> _populateCurrentUser(String user) async {
    if (user != null) {
      _currentUser = await _databaseService.getUser(user);
    }
  }
}
