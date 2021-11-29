﻿using EmailService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(Message email);
        Task SendEmailAsync(Message email);
    }
}