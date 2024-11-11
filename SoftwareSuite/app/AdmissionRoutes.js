define([], function () {
    return {
        defaultRoutePath: '/',
        routes: {
            '/': {
                url: "/",
                templateUrl: 'index.html',
                controller: '',
                dependencies: []
            },     
          

           // Student Results routes
            'Dashboard': {
                url: "/Dashboard",
                templateUrl: 'app/views/DashBoard.html',
                dependencies: ['controllers/DashBoardController','services/AdminService']
            },
			'maintainance': {
                url: "/maintainance",
                templateUrl: 'app/views/ServerUnderMaintainace.html',
                dependencies: []
            },

           //  Dashboard modules

            'Dashboard.DiplomaDashboard': {
                url: "/Diploma",
                templateUrl: 'app/views/DiplomaDashBoard.html',
                dependencies: ['controllers/DiplomaDashBoardController']
            },

            'Dashboard.TwshDashboard': {
                url: "/Twsh",
                templateUrl: 'app/views/TwshDashBoard.html',
                dependencies: ['controllers/TwshDashBoardController']
            },

            // Diploma Sub-modules
            'Dashboard.DiplomaDashboard.StudentResult': {
                url: "/StudentResult",
                templateUrl: 'app/views/Diploma/StudentResult.html',
                dependencies: ['controllers/Diploma/StudentResultController', 'services/StudentResultService', 'services/AdminServices/AdminService', 'services/PreExamination/PreExaminationService']
            },
            'Dashboard.DiplomaDashboard.StudentRequestForm': {
                url: "/StudentRequestForm",
                templateUrl: 'app/views/Diploma/StudentRequestForm.html',
                dependencies: ['controllers/Diploma/StudentRequestFormController', 'services/AdminServices/AdminService', 'services/PreExamination/PreExaminationService', 'services/BillDesk/paymentService']
            },

            'Dashboard.DiplomaDashboard.StudentConsolidatedResult': {
                url: "/StudentConsolidatedResult",
                templateUrl: 'app/views/Diploma/StudentConsolidatedResult.html',
                dependencies: ['controllers/Diploma/StudentConsolidatedResultController', 'services/StudentResultService', 'services/PreExamination/PreExaminationService']
            },
            'Dashboard.DiplomaDashboard.StudentFileUpload': {
                url: "/StudentFileUpload",
                templateUrl: 'app/views/Diploma/StudentFileUpload.html',
                dependencies: ['controllers/Diploma/StudentFileUploadController', 'services/PreExamination/PreExaminationService', 'services/BillDesk/paymentService']
            },

            'Dashboard.DiplomaDashboard.DiplomaFeePayment': {
                url: "/DiplomaFeePayment",
                templateUrl: 'app/views/Diploma/DiplomaFeePayment.html',
                dependencies: ['controllers/Diploma/DiplomaFeePaymentController',  'services/AdminServices/AdminService', 'services/Assessment/MarksEntryService', 'services/PreExamination/PreExaminationService', 'services/BillDesk/paymentService']
            },
            'Dashboard.DiplomaDashboard.Hallticket': {
                url: "/Hallticket",
                templateUrl: 'app/views/Diploma/Hallticket.html',
                dependencies: ['controllers/Diploma/HallticketController', 'services/Assessment/MarksEntryService', 'services/AdminServices/AdminService', 'services/PreExamination/PreExaminationService','services/BillDesk/paymentService']
            },
            'Dashboard.DiplomaDashboard.StudentAttendance': {
                url: "/StudentAttendance",
                templateUrl: 'app/views/Diploma/StudentAttendance.html',
                dependencies: ['controllers/Diploma/StudentAttendanceController', 'services/AcademicService', 'services/AdminServices/AdminService', 'services/PreExamination/PreExaminationService']
            },
            'Dashboard.DiplomaDashboard.StudentFeedBack': {
                url: "/StudentFeedBack",
                templateUrl: 'app/views/Diploma/StudentFeedback.html',
                dependencies: ['controllers/Diploma/StudentFeedbackController', 'services/PreExamination/AcademicService']
            },
            'Dashboard.DiplomaDashboard.StudentOnlineRequest': {
                url: "/StudentOnlineRequest",
                templateUrl: 'app/views/Diploma/StudentOnlineRequest.html',
                dependencies: ['controllers/Diploma/StudentOnlineRequestController',  'services/AdminServices/AdminService', 'services/PreExamination/PreExaminationService', 'services/BillDesk/paymentService']
            },
            'Dashboard.DiplomaDashboard.GenuinenessCheckForm': {
                url: "/GenuinenessCheckForm",
                templateUrl: 'app/views/Diploma/GenuinenessCheckForm.html',
                dependencies: ['controllers/Diploma/GenuinenessCheckFormController',  'services/AdminServices/AdminService', 'services/PreExamination/PreExaminationService', 'services/BillDesk/paymentService', 'directives/saDate']
            },

            'Dashboard.DiplomaDashboard.StudentConsolidatedResult': {
                url: "/StudentConsolidatedResult",
                templateUrl: 'app/views/Diploma/StudentConsolidatedResult.html',
                dependencies: ['controllers/Diploma/StudentConsolidatedResultController', 'services/StudentResultService', 'services/AdminServices/AdminService', 'services/PreExamination/PreExaminationService']
            },

            // Twsh Sub-modules

            'Dashboard.TwshDashboard.ShorthandResult': {
                url: "/ShorthandResult",
                templateUrl: 'app/views/TWSH/TypingCumShorthand.html',
                dependencies: ['controllers/TWSH/TypingCumShorthandResultsController', 'services/StudentResultService', 'services/AdminServices/AdminService']
            },

            'StudentMobileAppFeePayment': {
                url: "/StudentMobileAppFeePayment/StudentTypeId/:StudentTypeId/ExamMonthYearId/:ExamMonthYearId/Pin/:Pin",
                templateUrl: 'app/views/Diploma/StudentDiplomaFeePayment.html',
                dependencies: ['controllers/Diploma/StudentDiplomaFeePaymentController', 'services/Assessment/MarksEntryService', 'services/PreExamination/PreExaminationService', 'services/BillDesk/paymentService']
            },

            'StudentMobileAppResult': {
                url: "/StudentMobileAppResult/Scheme/:SchemeId/StudentType/:StudentTypeId/SemYear/:SemYearId/ExamType/:ExamTypeId/ExamMonthYear/:ExamMonthYearId/Pin/:Pin",
                templateUrl: 'app/views/StudentExamResults/StudentMobileAppResult.html',
                dependencies: ['controllers/StudentExamResults/StudentMobileAppResultController', 'services/StudentResultService']
            },
            'MobileAppHallticket': {
                url: "/MobileAppHallticket/StudentTypeId/:StudentTypeId/ExamMonthYearId/:ExamMonthYearId/Pin/:Pin",
                templateUrl: 'app/views/PreExamination/MobileAppHallticket.html',
                dependencies: ['controllers/PreExamination/MobileAppHallticketController', 'services/Assessment/MarksEntryService', 'services/PreExamination/PreExaminationService', 'services/BillDesk/paymentService']
               
            },

            'StudentMobileAppAttendance': {
                url: "/StudentMobileAppAttendance/Pin/:Pin",
                templateUrl: 'app/views/Diploma/StudentMobileAppAttendance.html',
                dependencies: ['controllers/Diploma/StudentMobileAppAttendanceController', 'services/AcademicService']
            },

            'StudentMobileAppFeedBack': {
                url: "/StudentMobileAppFeedBack",
                templateUrl: 'app/views/Diploma/StudentMobileAppFeedback.html',
                dependencies: ['controllers/Diploma/StudentMobileAppFeedbackController', 'services/PreExamination/AcademicService']
            },

            'StudentMobileAppNotifications': {
                url: "/StudentMobileAppNotifications",
                templateUrl: 'app/views/Diploma/StudentMobileAppNotifications.html',
                dependencies: ['controllers/Diploma/StudentMobileAppNotificationsController',  'services/AdminServices/AdminService']
            },

            'StudentMobileAppSyllabus': {
                url: "/StudentMobileAppSyllabus",
                templateUrl: 'app/views/Diploma/StudentMobileAppSyllabus.html',
                dependencies: ['controllers/Diploma/StudentMobileAppSyllabusController']
            },

            'StudentMobileAppSyllabusTimetable': {
                url: "/StudentMobileAppSyllabusTimetable",
                templateUrl: 'app/views/Diploma/StudentMobileAppSyllabusTimetable.html',
                dependencies: ['controllers/Diploma/StudentMobileAppSyllabusTimetableController', , 'services/PreExamination/PreExaminationService', 'directives/saDate', 'services/StudentResultService', 'directives/saDate']
            },

            
    
            

          
            // Payment Gateway response           
            'Dashboard.PaymentResponse': {
                url: "/PaymentResponse/:data",
                templateUrl: 'app/views/PaymentResponse.html',
                dependencies: ['controllers/PaymentGateway/PaymentResponseController', 'services/BillDesk/paymentService']
               
            },
            'PaymentGatewayResponse': {
                url: "/PaymentGatewayResponse/:data",
                templateUrl: 'app/views/PaymentGatewayResponse.html',
                dependencies: ['controllers/PaymentGateway/PaymentGatewayResponseController', 'services/BillDesk/paymentService', 'services/PreExamination/PreExaminationService']

            },
            'CertificateFeePaymentGatewayResponse': {
                url: "/CertificateFeePaymentGatewayResponse/:data",
                templateUrl: 'app/views/CertificateFeePaymentGatewayResponse.html',
                dependencies: ['controllers/PaymentGateway/CertificateFeePaymentGatewayResponseController', 'services/BillDesk/paymentService', 'services/PreExamination/PreExaminationService']

            },
        }
    };
});
