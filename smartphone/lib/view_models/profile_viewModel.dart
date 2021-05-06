import 'dart:io';

import 'package:cloud_firestore/cloud_firestore.dart';
import 'package:firebase_core/firebase_core.dart';
import 'package:smartphone/view_models/base_model.dart';

import '../locator.dart';

class ProfileView extends BaseModel {
  File _imagePicked;
  File get imagePicked => _imagePicked;

  Future<File> takeImageGallery() async {
    _imagePicked = await mediaService.pickImageGallery();
    notifyListeners();
    return _imagePicked;
  }

  Future<File> takeImageCamera() async {
    _imagePicked = await mediaService.pickImageCamera();
    notifyListeners();
    return _imagePicked;
  }

  Future<void> uploadAvatar() async {
    await profileServices.uploadAvatar(uid, _imagePicked);
    notifyListeners();
  }
}
