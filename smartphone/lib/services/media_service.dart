import 'dart:io';

import 'package:image_picker/image_picker.dart';

class MediaService {
  Future pickImageGallery() async {
    final picker = ImagePicker();
    final pickedFile = await picker.getImage(source: ImageSource.gallery);
    File image;

    if (pickedFile != null) {
      image = File(pickedFile.path);
    }

    return image;
  }

  Future pickImageCamera() async {
    final picker = ImagePicker();
    final pickedFile = await picker.getImage(source: ImageSource.camera);
    File image;

    if (pickedFile != null) {
      image = File(pickedFile.path);
    }

    return image;
  }
}
