﻿using CSharpDim.Kernel;
using CSharpDim.Messages;
using CSharpDim.Util;
using NetMQ.Sockets;

namespace CSharpDim.Information
{
	public class KernelInfoRequestHandler : IShellMessageHandler
	{
		public void HandleMessage(Message message, RouterSocket shellSocket, PublisherSocket ioPubSocket) {
			var kernelInfoRequest = JsonSerializer.Deserialize<KernelInfoRequest>(message.Content);

			var replyMessage = new Message {
				UUID = message.Header.Session,
				ParentHeader = message.Header,
				Header = MessageBuilder.CreateHeader(MessageTypeValues.KernelInfoReply, message.Header.Session),
				Content = JsonSerializer.Serialize(CreateKernelInfoReply())
			};

			Log.Info("Sending kernel_info_reply");
			MessageSender.Send(replyMessage, shellSocket);
		}

		private static KernelInfoReply CreateKernelInfoReply() {
			var kernelInfoReply = new KernelInfoReply {
				ProtocolVersion = "4.1",
				LanguageVersion = "0.0.1",
				IPythonVersion = "4.0.0",
				Language = "C#",
				Implementation = "C#dim",
				ImplementationVersion = "0.1"
			};

			return kernelInfoReply;
		}
	}
}