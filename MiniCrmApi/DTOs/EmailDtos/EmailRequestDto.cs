namespace MiniCrmApi.DTOs.EmailDtos
{
   public sealed record EmailRequestDto
   (
       string To, 
       string Subject, 
       string Body
   );
}
