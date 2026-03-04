namespace YAGO.World.Host.Controllers.Common
{
    public record ApiResponse<T>(
        bool Success,
        T? Data,
        ApiError? Error,
        ApiMeta? Meta,
        UpdatedEntities? UpdatedEntities,
        SlideResponse? Notification)
    {
        public static ApiResponse<T> CreateSuccess(
            T? data,
            UpdatedEntities? updatedEntities = null,
            SlideResponse? notification = null)
        {
            return new ApiResponse<T>(
                Success: true,
                Data: data,
                Error: null,
                Meta: null,
                UpdatedEntities: updatedEntities,
                Notification: notification);
        }
    }
}
