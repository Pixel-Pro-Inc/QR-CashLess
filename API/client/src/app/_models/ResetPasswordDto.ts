export interface ResetPasswordDto {
  password: string;
  confirmPassword: string;
  username: string;
  token: string;
}
