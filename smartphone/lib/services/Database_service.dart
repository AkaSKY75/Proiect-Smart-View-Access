import 'package:cloud_firestore/cloud_firestore.dart';
import 'package:smartphone/models/Angajat_dataModel.dart';

class DatabaseService {
  final CollectionReference _users =
      FirebaseFirestore.instance.collection('Angajat');
  Future<AngajatDataModel> getUser(String uid) async {
    try {
      DocumentSnapshot dataUser = await _users.doc(uid).get();
      Map<String, dynamic> allData;
      allData = dataUser.data();
      if (allData != null) {
        return AngajatDataModel.fromMap(allData);
      } else {
        print("NOT GOOd");
      }
    } catch (error) {
      print("NOT GOOd");
    }
  }
}
