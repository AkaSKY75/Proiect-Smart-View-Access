import 'package:cloud_firestore/cloud_firestore.dart';
import 'package:flutter/material.dart';
import 'package:crypto/crypto.dart';
import 'dart:convert';

class ChangePasswordService {
  FirebaseFirestore _firestore = FirebaseFirestore.instance;
  Future<void> changePass(String currentPass, String newPass,
      String confirmPass, String uid) async {
    String hashCurrent = hashPass(currentPass);
    String hashNewPass = hashPass(newPass);
    DocumentReference angajatReferance =
        _firestore.collection('Angajat').doc(uid);
    DocumentSnapshot docSnap =
        await _firestore.collection('Angajat').doc(uid).get();
    try {
      if (hashCurrent == docSnap.data()['parola'] && newPass == confirmPass) {
        angajatReferance.update({
          'parola': hashNewPass,
        });
      }
    } catch (error) {
      print("$error");
    }
  
  }

  String hashPass(String password) {
    List<int> passwordEncode = utf8.encode(password);
    Digest hash = sha256.convert(passwordEncode);
    return hash.toString();
  }
}
