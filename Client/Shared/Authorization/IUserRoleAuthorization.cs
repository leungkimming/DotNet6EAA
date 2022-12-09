namespace Client {
    public interface IUserRoleAuthorization {
        public bool IsAuthroize(string accessCode);
        Task SetAccessCodesAsync();
    }
}
