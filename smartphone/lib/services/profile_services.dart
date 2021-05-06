import 'dart:io';
import 'package:path/path.dart' as path;

import 'package:cloud_firestore/cloud_firestore.dart';
import 'package:firebase_storage/firebase_storage.dart';
import 'package:smartphone/models/Angajat_dataModel.dart';

class ProfileServices {
  FirebaseFirestore _firestore = FirebaseFirestore.instance;
  FirebaseStorage _ownStorage = FirebaseStorage.instance;

  Future<String> uploadImage(File imageFile, String uid) async {
    String downloadUrl;
    if (imageFile != null) {
      TaskSnapshot snapshot = await _ownStorage
          .ref()
          .child(uid)
          .child('Avatar')
          .child(path.basename(imageFile.path))
          .putFile(imageFile);
      if (snapshot.state == TaskState.success) {
        downloadUrl = await snapshot.ref.getDownloadURL();
      }
    }
    return downloadUrl;
  }

  Future<void> uploadAvatar(
    String uid,
    File imagePath,
  ) async {
    String angajatImagePath;
    String absPath;
    String getDownloadedImageUrl;
    if (imagePath != null) {
      absPath = path.basename(imagePath.path);
      angajatImagePath = '/$uid/Avatar/$absPath';
    }

    getDownloadedImageUrl = await uploadImage(imagePath, uid);
    DocumentReference angajatReferance =
        _firestore.collection('Angajat').doc(uid);
    try {
      angajatReferance.update({
        'avatar': angajatImagePath,
      });
    } catch (error) {
      print("Error adding document: " + error);
    }

    AngajatDataModel.fromMap({}, getDownloadedImageUrl,uid);
  }
}
