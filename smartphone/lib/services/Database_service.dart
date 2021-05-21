import 'package:cloud_firestore/cloud_firestore.dart';
import 'package:firebase_storage/firebase_storage.dart';
import 'package:smartphone/models/Angajat_dataModel.dart';

class DatabaseService {
  final CollectionReference _users =
      FirebaseFirestore.instance.collection('Auth');
  DocumentReference angajatRef;
  FirebaseStorage _ownStorage = FirebaseStorage.instance;

  Future<AngajatDataModel> getUser(String uid) async {
    String downUrl;
    try {
      Map<String, dynamic> testData;
      DocumentSnapshot dataAuth = await _users.doc(uid).get();
      testData = dataAuth.data();
      print("DATELEEEEEEEE =========== $testData");
      angajatRef = FirebaseFirestore.instance.doc(dataAuth.data()['path']);
      DocumentSnapshot dataUser = await angajatRef.get();
      Map<String, dynamic> allData;
      downUrl = '';
      allData = dataUser.data();
      if (dataUser['avatar'] != '') {
        downUrl = (await _ownStorage.ref(dataUser['avatar']).getDownloadURL())
            .toString();
      }
      if (allData != null) {
        return AngajatDataModel.fromMap(allData, downUrl, uid);
      } else {
        print("NOT GOOd");
      }
    } catch (error) {
      print("NOT GOOdSHIT:$error");
    }
  }
}
