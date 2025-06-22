export class SignUpRequest {
  constructor(username, email, password, name, fatherName, motherName, dateOfBirth, documentNumber, phone) {
      this.username = username;
      this.email = email;
      this.password = password;
      this.name = name;
      this.fatherName = fatherName;
      this.motherName = motherName;
      this.dateOfBirth = dateOfBirth;
      this.documentNumber = documentNumber;
      this.phone = phone;
  }
}