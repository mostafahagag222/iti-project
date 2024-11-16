export interface IUser {
  userId: string;
  expiration: Date;
  username: string;
  profilePicture: string;
  email: string;
  mobileNumber: string;
  token: string;
  roles: string[];
  gender: string;
}
