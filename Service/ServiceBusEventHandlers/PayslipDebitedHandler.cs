//// Uncomment all lines to enable NServiceBus Demo

//using Messages;
//using NServiceBus;
//using NServiceBus.Logging;
//using Data;

//namespace Service.ServiceBusEventHandlers {
//    public class PayslipDebitedHandler : IHandleMessages<PayslipDebited> {
//        static readonly ILog log = LogManager.GetLogger<PayslipDebitedHandler>();
//        public IUnitOfWork _unitOfWork;
//        public PayslipDebitedHandler(IUnitOfWork unitOfWork) {
//            this._unitOfWork = unitOfWork;
//        }
//        public async Task Handle(PayslipDebited message, IMessageHandlerContext context) {
//            log.Info($"Received PayslipDebited, UserId = {message.UserId}, " +
//                $"Date {message.PayslipDate}, " +
//                $"Amount {message.Amount}. ");
//            var repository = _unitOfWork.UserRepository();
//            var user = await repository.GetAsyncWithPayslip(_ => _.Id == message.UserId);
//            if (user != null) {
//                user.SendPayslipLetter(message.PayslipDate, message.letter);
//                await _unitOfWork.SaveChangesAsync();
//            }
//            return;
//        }
//    }
//}