public enum ApplyType
{
    NONE,

    #region LIFE
    // 주민등록등본
    ISSUANCE_CERTIFIED_RESIDENT_REGISTRATION,
    // 주민 등록증
    ISSUANCE_REGISTRATION_CARD,
    // 여권
    ISSUANCE_PASSPORT,
    // 상담
    COUNSEL,

    VIDEOCALL_COUNSEL_APPLY,
    CHAT_COUNSEL_APPLY,
    #endregion

    #region STUDY
    // 재학 증명서
    ISSUANCE_CERTIFICATE_ENROLLMENT,
    // 졸업 증명서
    ISSUANCE_CERTIFICATE_GRADUATION,
    // 휴학 증명서
    ISSUANCE_CERIFICATE_LEAVEOFABSENCE,
    // 납입 증명서
    ISSUANCE_CERTIFICATE_PAYMENT,
    #endregion

    #region JOB
    // 국가기술자격증
    TECHNICAL_QUALIFICATION,
    // 취업 특강
    EMPLOYMENT_SPECIAL_LECTURE,
    // 고용보험 이력
    ISSUANCE_EMPLOYMENT_ISURANCE_HISTORY,
    // 보건증
    ISSUACNE_CERTIFICATE_HEALTH,
    #endregion

    #region ETC
    MY_PAGE,
    COUNSEL_LOG,
    #endregion
}