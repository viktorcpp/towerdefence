using UnityEngine;
using System.Collections;

public class FPSOutput : Singleton<FPSOutput>
{
	[SerializeField]
	private float      up_interval  = 0.5f;
	[SerializeField]
	private bool       locked       = false;
	[SerializeField]
	private Color      color_norm   = Color.green;
	[SerializeField]
	private Color      color_middle = Color.yellow;
	[SerializeField]
	private Color      color_low    = Color.red;
	private float      accum        = 0; // FPS accumulated over the interval
	private int        frames       = 0; // Frames drawn over the interval
	private float      time_left    = 0; // Left time for current interval
	[SerializeField]
	private SpriteText label        = null;
	private float      fps          = 0.0f;
    private Vector3    pos_show     = Vector3.zero;
    private Vector3    pos_hide     = Vector3.zero;

	//****************************************************************
	public float GetFps{ get{ return fps; } }
    public bool  Lock
    {
        set
        {
            locked = value;
            if( locked )
            {
                label.transform.localPosition = pos_hide;
            }
            else
            {
                label.transform.localPosition = pos_show;
            }
        }
    }

	//****************************************************************
	public void _Start()
	{
        pos_show   = label.transform.localPosition;
        pos_hide   = label.transform.localPosition;
        pos_hide.z = 3000.0f;

		time_left = up_interval;
		label.SetColor( color_norm );

        if( !Options.Instance.ValueB( "is-dev-showfps" ) )
        {
            this.Lock = true;
        }
	}

	//****************************************************************
	private void Update()
	{
		if( locked || label == null ) return;

		time_left -= Time.deltaTime;
		accum     += Time.timeScale/Time.deltaTime;
		++frames;

		// Interval ended - update GUI text and start new interval
		if( time_left <= 0.0 )
		{
			// display two fractional digits (f2 format)
			fps    = accum/frames;
			string format = System.String.Format( "{0:F1}", fps );
			label.Text   = format;

			if( fps < 30 )
			{
				if( fps < 30 && fps > 10 )
					label.SetColor( color_middle );
				else
					label.SetColor( color_low );
			}
			else
			{
				label.SetColor( color_norm );
			}

			time_left = up_interval;
			accum     = 0.0F;
			frames    = 0;
		}
	}
}