using api.Models;

namespace api.Services {
    public static class AccountService {
        public static ApiResult<bool> AmILoggedIn()
        {
            var result = new ApiResult<bool>();

            result.Data = true;

            return result;
        }
    }
}