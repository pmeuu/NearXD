﻿using NearCompanion.Client.Services.Interfaces;
using NearCompanion.Shared;
using System.Net.Http.Json;

namespace NearCompanion.Client.Services
{
    public class BlockService : IBlockService
    {
        public BlockService(HttpClient http)
        {
            httpClient = http;
        }

        private HttpClient httpClient;
        private bool keepQueryingBlocks = true;

        public event EventHandler<List<BlockModel>>? NewBlocksReceivedEvent;

        public async Task StartReceivingBlocks()
        {
            var finalBlockResponse = await httpClient.GetFromJsonAsync<Response<BlockModel>>("block");

            if (finalBlockResponse == null || !finalBlockResponse.Success || finalBlockResponse.Data == null)
            {
                // No final block, server might be down, keep polling every few seconds
                await Task.Delay(5000);
                _ = StartReceivingBlocks();
                return;
            }

            ulong previousHeight = finalBlockResponse.Data.Height + 1;

            NewBlocksReceivedEvent?.Invoke(null, new List<BlockModel>() { finalBlockResponse.Data });

            while (keepQueryingBlocks)
            {
                var latestBlocksResponse = await httpClient.GetFromJsonAsync<Response<List<BlockModel>>>($"block/{previousHeight}");

                if (latestBlocksResponse == null || 
                    !latestBlocksResponse.Success || 
                    latestBlocksResponse.Data == null || 
                    latestBlocksResponse.Data.Count == 0)
                {
                    await Task.Delay(2000);
                    continue;
                }

                foreach (var newBlock in latestBlocksResponse.Data)
                {
                    //_ = AddBlockAndHandleAnimations(newBlock, (int)newBlock.LengthMs);
                    previousHeight++;
                }

                NewBlocksReceivedEvent?.Invoke(null, latestBlocksResponse.Data);

                await Task.Delay(5000);
            }
        }

        public Task StopReceivingBlocks()
        {
            throw new NotImplementedException();
        }
    }
}