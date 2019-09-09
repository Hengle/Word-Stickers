public class LanguageSettings : ELLanguageConfig<LanguageSettings>
{
	public const int CONST_TEXT_SHARE_TITLE = 0;

	public const int CONST_TEXT_SHARE_MASSAGE = 1;

	public const int CONST_TEXT_EMOJI = 10;

	public new void LoadSettingsCompleted()
	{
		base.LoadSettingsCompleted();
	}
}
