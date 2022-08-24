﻿namespace NearCompanion.Server.Helpers
{
    public static class RpcJsonHelpers
    {
        private static string AppId => "NearCompanion";
        private static string Comma => ",";
        private static string LeftBracket => "{";
        private static string RightBracket => "}";
        private static string JsonRpcVersion => "\"jsonrpc\": \"2.0\"";
        private static string RequestId => $"\"id\": \"{AppId}\"";
        private static string BlockMethod => "\"method\": \"block\"";
        private static string FinalBlock => "\"params\": { \"finality\": \"final\" }";
        private static string BlockHeight(ulong blockHeight) => $"\"params\": {{ \"block_id\": {blockHeight} }}";

        public static string GetLatestFinalBlockJson()
        {
            return LeftBracket            +
                   JsonRpcVersion + Comma +
                   RequestId      + Comma +
                   BlockMethod    + Comma +
                   FinalBlock             +
                   RightBracket;
        }

        public static string GetBlockJson(ulong blockHeight)
        {
            return LeftBracket              +
                   JsonRpcVersion   + Comma +
                   RequestId        + Comma +
                   BlockMethod      + Comma +
                   BlockHeight(blockHeight) +
                   RightBracket;
        }
    }
}
