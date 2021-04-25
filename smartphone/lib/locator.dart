import 'package:get_it/get_it.dart';
import 'package:smartphone/services/Database_service.dart';
import 'package:smartphone/services/login_service.dart';
import 'package:smartphone/services/profile_services.dart';
import 'package:stacked_services/stacked_services.dart';

GetIt locator = GetIt.instance;

void setupLocator() {
  locator.registerLazySingleton(() => NavigationService());
  locator.registerLazySingleton(() => LoginServices());
  locator.registerLazySingleton(() => ProfileServices());
  locator.registerLazySingleton(() => DatabaseService());
}
