// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 08-18-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-23-2015
// ***********************************************************************
// <copyright file="SurveyMonkeyService.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.SurveyMonkey
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::SurveyMonkey;

    #endregion

    /// <summary>
    /// Class SurveyMonkeyService.
    /// </summary>
    public class SurveyMonkeyService
    {
        #region Fields

        /// <summary>
        /// The API
        /// </summary>
        private readonly SurveyMonkeyApi api;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SurveyMonkeyService" /> class.
        /// </summary>
        /// <param name="surveyMonkeyKey">The survey monkey key.</param>
        /// <param name="surveyMonkeyToken">The survey monkey token.</param>
        public SurveyMonkeyService(string surveyMonkeyKey, string surveyMonkeyToken)
        {
            this.api = new SurveyMonkeyApi(surveyMonkeyKey, surveyMonkeyToken);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the available surveys.
        /// </summary>
        /// <returns>IEnumerable&lt;Survey&gt;.</returns>
        public IEnumerable<Survey> GetAvailableSurveys()
        {
            List<Survey> surveys;
            //// Get all user voice surveys
            var userVoiceSurveys = this.api.GetSurveyList(new GetSurveyListSettings { Title = "User Voice", OrderAsc = false });
            var regularSurveys =
                this.api.GetSurveyList(
                    new GetSurveyListSettings { StartDate = DateTime.Now.AddYears(-1), OrderAsc = false });
            if (userVoiceSurveys != null)
            {
                regularSurveys =
                    regularSurveys.Where(
                        survey => userVoiceSurveys.All(uvSurvey => survey.SurveyId != uvSurvey.SurveyId)).ToList();
            }

            if (userVoiceSurveys != null)
            {
                surveys = userVoiceSurveys.Concat(regularSurveys).ToList();
                this.api.FillMissingSurveyInformation(surveys);
                return surveys;
            }

            if (regularSurveys == null)
            {
                return null;
            }

            this.api.FillMissingSurveyInformation(regularSurveys);
            return regularSurveys;
        }

        #endregion
    }
}