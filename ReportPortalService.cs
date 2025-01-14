﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using ReportPortal.Client.Models;
using ReportPortalNUnitLog4netClient.Core;
using ReportPortalNUnitLog4netClient.Models;

namespace ReportPortalNUnitLog4netClient
{
    public class ReportPortalService : IReportPortalService
    {
        private static readonly object Padlock = new object();
        private static ReportPortalService _reportPortalService;
        private IReportPortalService _iReportPortalService;

        private ReportPortalService()
        {
        }

        public static ReportPortalService Instance
        {
            get
            {
                if (_reportPortalService == null)
                {
                    lock (Padlock)
                    {
                        if (_reportPortalService == null)
                        {
                            _reportPortalService = new ReportPortalService();
                        }
                    }
                }
                return _reportPortalService;
            }
        }

        public ReportPortalService Init(RpConfiguration rpConfiguration, IDictionary<Type, string> defectsMapping = null)
        {
            if (rpConfiguration.IsEnabled)
            {
                _iReportPortalService = new ReportPortalEnabled(rpConfiguration, defectsMapping);
            }
            else
            {
                _iReportPortalService = new ReportPortalDisabled();
            }
            return this;
        }

        public IReportPortalService StartLaunch(List<string> tags = null)
        {
            return _iReportPortalService.StartLaunch(tags);
        }

        public IReportPortalService FinishLaunch()
        {
            return _iReportPortalService?.FinishLaunch();
        }

        public IReportPortalService StartTest(TestContext.TestAdapter test, string suiteName, string subSuite, List<string> tags, string testCodeId = null, List<string> tmsIds = null)
        {
            return _iReportPortalService.StartTest(test, suiteName, subSuite, tags, testCodeId, tmsIds);
        }

        public IReportPortalService FinishTest(TestContext.TestAdapter test, TestStatus status, string errorMessage, List<string> ticketIds)
        {
            return _iReportPortalService.FinishTest(test, status, errorMessage, ticketIds);
        }

        public IReportPortalService Log(TestContext.TestAdapter test, LogLevel level, DateTime time, string text, Attach attach = null)
        {
            return _iReportPortalService.Log(test, level, time, text, attach);
        }

        public Launch GetLaunch()
        {
            return _iReportPortalService.GetLaunch();
        }
    }
}
