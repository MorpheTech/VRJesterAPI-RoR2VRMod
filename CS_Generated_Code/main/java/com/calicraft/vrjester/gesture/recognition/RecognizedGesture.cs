namespace com.calicraft.vrjester.gesture.recognition
{
	public record class RecognizedGesture(string gestureName, string hmdGesture, string rcGesture, string lcGesture)
	{

		public bool Found
		{
			get
			{
				return gestureName != null;
			}
		}
	}

}
