import 'package:smartphone/models/Angajat_dataModel.dart';
import 'package:smartphone/services/Database_service.dart';
import 'package:smartphone/services/login_service.dart';
import 'package:smartphone/services/profile_services.dart';
import 'package:stacked/stacked.dart';
import 'package:stacked_services/stacked_services.dart';

import '../locator.dart';

class BaseModel extends BaseViewModel {
  final NavigationService navigationService = locator<NavigationService>();
  final LoginServices loginServices = locator<LoginServices>();
  final ProfileServices profileServices = locator<ProfileServices>();
  final DatabaseService databaseService = locator<DatabaseService>();

  AngajatDataModel getCurrentUser() {
    return loginServices.currentUser;
  }

  String get nume => getCurrentUser().nume;
  String get avatar => getCurrentUser().avatar;
  String get prenume => getCurrentUser().prenume;
  int get cnp => getCurrentUser().cnp;
  String get departament => getCurrentUser().departament;
  int get etaj => getCurrentUser().etaj;
  int get birou => getCurrentUser().birou;
  int get loc => getCurrentUser().loc;
  String get nr_inmatriculare => getCurrentUser().numar_inmatriculare;
}
