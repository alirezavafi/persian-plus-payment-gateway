﻿// Copyright (c) Persian.Plus.PaymentGateway.Core. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC License, Version 3.0. See License.txt in the project root for license information.

using Persian.Plus.PaymentGateway.Core.Storage.Abstractions.Models;
using Persian.Plus.PaymentGateway.Storage.EntityFrameworkCore.Domain;

namespace Persian.Plus.PaymentGateway.Storage.EntityFrameworkCore.Internal
{
    internal static class Mapper
    {
        public static PaymentEntity ToEntity(this Payment model)
        {
            return new PaymentEntity
            {
                TrackingNumber = model.TrackingNumber,
                Amount = model.Amount,
                Token = model.Token,
                TransactionCode = model.TransactionCode,
                GatewayName = model.GatewayName,
                GatewayAccountName = model.GatewayAccountName,
                IsCompleted = model.IsCompleted,
                IsPaid = model.IsPaid
            };
        }

        public static void ToEntity(Payment model, PaymentEntity entity)
        {
            entity.TrackingNumber = model.TrackingNumber;
            entity.Amount = model.Amount;
            entity.Token = model.Token;
            entity.TransactionCode = model.TransactionCode;
            entity.GatewayName = model.GatewayName;
            entity.GatewayAccountName = model.GatewayAccountName;
            entity.IsCompleted = model.IsCompleted;
            entity.IsPaid = model.IsPaid;
        }

        public static Payment ToModel(this PaymentEntity entity)
        {
            return new Payment
            {
                Id = entity.Id,
                TrackingNumber = entity.TrackingNumber,
                Amount = entity.Amount,
                Token = entity.Token,
                TransactionCode = entity.TransactionCode,
                GatewayName = entity.GatewayName,
                GatewayAccountName = entity.GatewayAccountName,
                IsCompleted = entity.IsCompleted,
                IsPaid = entity.IsPaid,
                CreatedOn = entity.CreatedOn,
            };
        }

        public static TransactionEntity ToEntity(this Transaction model)
        {
            return new TransactionEntity
            {
                Amount = model.Amount,
                Type = model.Type,
                IsSucceed = model.IsSucceed,
                Message = model.Message,
                AdditionalData = model.AdditionalData,
                PaymentId = model.PaymentId
            };
        }

        public static void ToEntity(Transaction model, TransactionEntity entity)
        {
            entity.Amount = model.Amount;
            entity.Type = model.Type;
            entity.IsSucceed = model.IsSucceed;
            entity.Message = model.Message;
            entity.AdditionalData = model.AdditionalData;
        }

        public static Transaction ToModel(this TransactionEntity entity)
        {
            return new Transaction
            {
                Id = entity.Id,
                Amount = entity.Amount,
                Type = entity.Type,
                IsSucceed = entity.IsSucceed,
                Message = entity.Message,
                AdditionalData = entity.AdditionalData,
                PaymentId = entity.PaymentId,
                DateTime = entity.UpdatedOn ?? entity.CreatedOn,
            };
        }
    }
}
