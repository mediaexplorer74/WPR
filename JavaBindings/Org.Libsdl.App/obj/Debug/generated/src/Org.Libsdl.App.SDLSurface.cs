//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable restore
using System;
using System.Collections.Generic;
using Android.Runtime;
using Java.Interop;

namespace Org.Libsdl.App {

	// Metadata.xml XPath class reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']"
	[global::Android.Runtime.Register ("org/libsdl/app/SDLSurface", DoNotGenerateAcw=true)]
	public partial class SDLSurface : global::Android.Views.SurfaceView, global::Android.Hardware.ISensorEventListener, global::Android.Views.ISurfaceHolderCallback, global::Android.Views.View.IOnKeyListener, global::Android.Views.View.IOnTouchListener {

		// Metadata.xml XPath field reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/field[@name='mDisplay']"
		[Register ("mDisplay")]
		protected global::Android.Views.Display MDisplay {
			get {
				const string __id = "mDisplay.Landroid/view/Display;";

				var __v = _members.InstanceFields.GetObjectValue (__id, this);
				return global::Java.Lang.Object.GetObject<global::Android.Views.Display> (__v.Handle, JniHandleOwnership.TransferLocalRef);
			}
			set {
				const string __id = "mDisplay.Landroid/view/Display;";

				IntPtr native_value = global::Android.Runtime.JNIEnv.ToLocalJniHandle (value);
				try {
					_members.InstanceFields.SetValue (__id, this, new JniObjectReference (native_value));
				} finally {
					global::Android.Runtime.JNIEnv.DeleteLocalRef (native_value);
				}
			}
		}


		// Metadata.xml XPath field reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/field[@name='mHeight']"
		[Register ("mHeight")]
		protected float MHeight {
			get {
				const string __id = "mHeight.F";

				var __v = _members.InstanceFields.GetSingleValue (__id, this);
				return __v;
			}
			set {
				const string __id = "mHeight.F";

				try {
					_members.InstanceFields.SetValue (__id, this, value);
				} finally {
				}
			}
		}


		// Metadata.xml XPath field reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/field[@name='mIsSurfaceReady']"
		[Register ("mIsSurfaceReady")]
		public bool MIsSurfaceReady {
			get {
				const string __id = "mIsSurfaceReady.Z";

				var __v = _members.InstanceFields.GetBooleanValue (__id, this);
				return __v;
			}
			set {
				const string __id = "mIsSurfaceReady.Z";

				try {
					_members.InstanceFields.SetValue (__id, this, value);
				} finally {
				}
			}
		}


		// Metadata.xml XPath field reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/field[@name='mSensorManager']"
		[Register ("mSensorManager")]
		protected global::Android.Hardware.SensorManager MSensorManager {
			get {
				const string __id = "mSensorManager.Landroid/hardware/SensorManager;";

				var __v = _members.InstanceFields.GetObjectValue (__id, this);
				return global::Java.Lang.Object.GetObject<global::Android.Hardware.SensorManager> (__v.Handle, JniHandleOwnership.TransferLocalRef);
			}
			set {
				const string __id = "mSensorManager.Landroid/hardware/SensorManager;";

				IntPtr native_value = global::Android.Runtime.JNIEnv.ToLocalJniHandle (value);
				try {
					_members.InstanceFields.SetValue (__id, this, new JniObjectReference (native_value));
				} finally {
					global::Android.Runtime.JNIEnv.DeleteLocalRef (native_value);
				}
			}
		}


		// Metadata.xml XPath field reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/field[@name='mWidth']"
		[Register ("mWidth")]
		protected float MWidth {
			get {
				const string __id = "mWidth.F";

				var __v = _members.InstanceFields.GetSingleValue (__id, this);
				return __v;
			}
			set {
				const string __id = "mWidth.F";

				try {
					_members.InstanceFields.SetValue (__id, this, value);
				} finally {
				}
			}
		}

		static readonly JniPeerMembers _members = new XAPeerMembers ("org/libsdl/app/SDLSurface", typeof (SDLSurface));

		internal static IntPtr class_ref {
			get { return _members.JniPeerType.PeerReference.Handle; }
		}

		[global::System.Diagnostics.DebuggerBrowsable (global::System.Diagnostics.DebuggerBrowsableState.Never)]
		[global::System.ComponentModel.EditorBrowsable (global::System.ComponentModel.EditorBrowsableState.Never)]
		public override global::Java.Interop.JniPeerMembers JniPeerMembers {
			get { return _members; }
		}

		[global::System.Diagnostics.DebuggerBrowsable (global::System.Diagnostics.DebuggerBrowsableState.Never)]
		[global::System.ComponentModel.EditorBrowsable (global::System.ComponentModel.EditorBrowsableState.Never)]
		protected override IntPtr ThresholdClass {
			get { return _members.JniPeerType.PeerReference.Handle; }
		}

		[global::System.Diagnostics.DebuggerBrowsable (global::System.Diagnostics.DebuggerBrowsableState.Never)]
		[global::System.ComponentModel.EditorBrowsable (global::System.ComponentModel.EditorBrowsableState.Never)]
		protected override global::System.Type ThresholdType {
			get { return _members.ManagedPeerType; }
		}

		protected SDLSurface (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer)
		{
		}

		// Metadata.xml XPath constructor reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/constructor[@name='SDLSurface' and count(parameter)=1 and parameter[1][@type='android.content.Context']]"
		[Register (".ctor", "(Landroid/content/Context;)V", "")]
		public unsafe SDLSurface (global::Android.Content.Context context) : base (IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
		{
			const string __id = "(Landroid/content/Context;)V";

			if (((global::Java.Lang.Object) this).Handle != IntPtr.Zero)
				return;

			try {
				JniArgumentValue* __args = stackalloc JniArgumentValue [1];
				__args [0] = new JniArgumentValue ((context == null) ? IntPtr.Zero : ((global::Java.Lang.Object) context).Handle);
				var __r = _members.InstanceMethods.StartCreateInstance (__id, ((object) this).GetType (), __args);
				SetHandle (__r.Handle, JniHandleOwnership.TransferLocalRef);
				_members.InstanceMethods.FinishCreateInstance (__id, this, __args);
			} finally {
				global::System.GC.KeepAlive (context);
			}
		}

		static Delegate cb_getNativeSurface;
#pragma warning disable 0169
		static Delegate GetGetNativeSurfaceHandler ()
		{
			if (cb_getNativeSurface == null)
				cb_getNativeSurface = JNINativeWrapper.CreateDelegate (new _JniMarshal_PP_L (n_GetNativeSurface));
			return cb_getNativeSurface;
		}

		static IntPtr n_GetNativeSurface (IntPtr jnienv, IntPtr native__this)
		{
			var __this = global::Java.Lang.Object.GetObject<global::Org.Libsdl.App.SDLSurface> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			return JNIEnv.ToLocalJniHandle (__this.NativeSurface);
		}
#pragma warning restore 0169

		public virtual unsafe global::Android.Views.Surface NativeSurface {
			// Metadata.xml XPath method reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/method[@name='getNativeSurface' and count(parameter)=0]"
			[Register ("getNativeSurface", "()Landroid/view/Surface;", "GetGetNativeSurfaceHandler")]
			get {
				const string __id = "getNativeSurface.()Landroid/view/Surface;";
				try {
					var __rm = _members.InstanceMethods.InvokeVirtualObjectMethod (__id, this, null);
					return global::Java.Lang.Object.GetObject<global::Android.Views.Surface> (__rm.Handle, JniHandleOwnership.TransferLocalRef);
				} finally {
				}
			}
		}

		static Delegate cb_enableSensor_IZ;
#pragma warning disable 0169
		static Delegate GetEnableSensor_IZHandler ()
		{
			if (cb_enableSensor_IZ == null)
				cb_enableSensor_IZ = JNINativeWrapper.CreateDelegate (new _JniMarshal_PPIZ_V (n_EnableSensor_IZ));
			return cb_enableSensor_IZ;
		}

		static void n_EnableSensor_IZ (IntPtr jnienv, IntPtr native__this, int sensortype, bool enabled)
		{
			var __this = global::Java.Lang.Object.GetObject<global::Org.Libsdl.App.SDLSurface> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			__this.EnableSensor (sensortype, enabled);
		}
#pragma warning restore 0169

		// Metadata.xml XPath method reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/method[@name='enableSensor' and count(parameter)=2 and parameter[1][@type='int'] and parameter[2][@type='boolean']]"
		[Register ("enableSensor", "(IZ)V", "GetEnableSensor_IZHandler")]
		public virtual unsafe void EnableSensor (int sensortype, bool enabled)
		{
			const string __id = "enableSensor.(IZ)V";
			try {
				JniArgumentValue* __args = stackalloc JniArgumentValue [2];
				__args [0] = new JniArgumentValue (sensortype);
				__args [1] = new JniArgumentValue (enabled);
				_members.InstanceMethods.InvokeVirtualVoidMethod (__id, this, __args);
			} finally {
			}
		}

		static Delegate cb_handlePause;
#pragma warning disable 0169
		static Delegate GetHandlePauseHandler ()
		{
			if (cb_handlePause == null)
				cb_handlePause = JNINativeWrapper.CreateDelegate (new _JniMarshal_PP_V (n_HandlePause));
			return cb_handlePause;
		}

		static void n_HandlePause (IntPtr jnienv, IntPtr native__this)
		{
			var __this = global::Java.Lang.Object.GetObject<global::Org.Libsdl.App.SDLSurface> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			__this.HandlePause ();
		}
#pragma warning restore 0169

		// Metadata.xml XPath method reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/method[@name='handlePause' and count(parameter)=0]"
		[Register ("handlePause", "()V", "GetHandlePauseHandler")]
		public virtual unsafe void HandlePause ()
		{
			const string __id = "handlePause.()V";
			try {
				_members.InstanceMethods.InvokeVirtualVoidMethod (__id, this, null);
			} finally {
			}
		}

		static Delegate cb_handleResume;
#pragma warning disable 0169
		static Delegate GetHandleResumeHandler ()
		{
			if (cb_handleResume == null)
				cb_handleResume = JNINativeWrapper.CreateDelegate (new _JniMarshal_PP_V (n_HandleResume));
			return cb_handleResume;
		}

		static void n_HandleResume (IntPtr jnienv, IntPtr native__this)
		{
			var __this = global::Java.Lang.Object.GetObject<global::Org.Libsdl.App.SDLSurface> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			__this.HandleResume ();
		}
#pragma warning restore 0169

		// Metadata.xml XPath method reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/method[@name='handleResume' and count(parameter)=0]"
		[Register ("handleResume", "()V", "GetHandleResumeHandler")]
		public virtual unsafe void HandleResume ()
		{
			const string __id = "handleResume.()V";
			try {
				_members.InstanceMethods.InvokeVirtualVoidMethod (__id, this, null);
			} finally {
			}
		}

		static Delegate cb_onAccuracyChanged_Landroid_hardware_Sensor_I;
#pragma warning disable 0169
		static Delegate GetOnAccuracyChanged_Landroid_hardware_Sensor_IHandler ()
		{
			if (cb_onAccuracyChanged_Landroid_hardware_Sensor_I == null)
				cb_onAccuracyChanged_Landroid_hardware_Sensor_I = JNINativeWrapper.CreateDelegate (new _JniMarshal_PPLI_V (n_OnAccuracyChanged_Landroid_hardware_Sensor_I));
			return cb_onAccuracyChanged_Landroid_hardware_Sensor_I;
		}

		static void n_OnAccuracyChanged_Landroid_hardware_Sensor_I (IntPtr jnienv, IntPtr native__this, IntPtr native_sensor, int native_accuracy)
		{
			var __this = global::Java.Lang.Object.GetObject<global::Org.Libsdl.App.SDLSurface> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			var sensor = global::Java.Lang.Object.GetObject<global::Android.Hardware.Sensor> (native_sensor, JniHandleOwnership.DoNotTransfer);
			var accuracy = (global::Android.Hardware.SensorStatus) native_accuracy;
			__this.OnAccuracyChanged (sensor, accuracy);
		}
#pragma warning restore 0169

		// Metadata.xml XPath method reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/method[@name='onAccuracyChanged' and count(parameter)=2 and parameter[1][@type='android.hardware.Sensor'] and parameter[2][@type='int']]"
		[Register ("onAccuracyChanged", "(Landroid/hardware/Sensor;I)V", "GetOnAccuracyChanged_Landroid_hardware_Sensor_IHandler")]
		public virtual unsafe void OnAccuracyChanged (global::Android.Hardware.Sensor sensor, [global::Android.Runtime.GeneratedEnum] global::Android.Hardware.SensorStatus accuracy)
		{
			const string __id = "onAccuracyChanged.(Landroid/hardware/Sensor;I)V";
			try {
				JniArgumentValue* __args = stackalloc JniArgumentValue [2];
				__args [0] = new JniArgumentValue ((sensor == null) ? IntPtr.Zero : ((global::Java.Lang.Object) sensor).Handle);
				__args [1] = new JniArgumentValue ((int) accuracy);
				_members.InstanceMethods.InvokeVirtualVoidMethod (__id, this, __args);
			} finally {
				global::System.GC.KeepAlive (sensor);
			}
		}

		static Delegate cb_onCapturedPointerEvent_Landroid_view_MotionEvent_;
#pragma warning disable 0169
		static Delegate GetOnCapturedPointerEvent_Landroid_view_MotionEvent_Handler ()
		{
			if (cb_onCapturedPointerEvent_Landroid_view_MotionEvent_ == null)
				cb_onCapturedPointerEvent_Landroid_view_MotionEvent_ = JNINativeWrapper.CreateDelegate (new _JniMarshal_PPL_Z (n_OnCapturedPointerEvent_Landroid_view_MotionEvent_));
			return cb_onCapturedPointerEvent_Landroid_view_MotionEvent_;
		}

		static bool n_OnCapturedPointerEvent_Landroid_view_MotionEvent_ (IntPtr jnienv, IntPtr native__this, IntPtr native_e)
		{
			var __this = global::Java.Lang.Object.GetObject<global::Org.Libsdl.App.SDLSurface> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			var e = global::Java.Lang.Object.GetObject<global::Android.Views.MotionEvent> (native_e, JniHandleOwnership.DoNotTransfer);
			bool __ret = __this.OnCapturedPointerEvent (e);
			return __ret;
		}
#pragma warning restore 0169

		// Metadata.xml XPath method reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/method[@name='onCapturedPointerEvent' and count(parameter)=1 and parameter[1][@type='android.view.MotionEvent']]"
		[Register ("onCapturedPointerEvent", "(Landroid/view/MotionEvent;)Z", "GetOnCapturedPointerEvent_Landroid_view_MotionEvent_Handler")]
		public virtual unsafe bool OnCapturedPointerEvent (global::Android.Views.MotionEvent e)
		{
			const string __id = "onCapturedPointerEvent.(Landroid/view/MotionEvent;)Z";
			try {
				JniArgumentValue* __args = stackalloc JniArgumentValue [1];
				__args [0] = new JniArgumentValue ((e == null) ? IntPtr.Zero : ((global::Java.Lang.Object) e).Handle);
				var __rm = _members.InstanceMethods.InvokeVirtualBooleanMethod (__id, this, __args);
				return __rm;
			} finally {
				global::System.GC.KeepAlive (e);
			}
		}

		static Delegate cb_onKey_Landroid_view_View_ILandroid_view_KeyEvent_;
#pragma warning disable 0169
		static Delegate GetOnKey_Landroid_view_View_ILandroid_view_KeyEvent_Handler ()
		{
			if (cb_onKey_Landroid_view_View_ILandroid_view_KeyEvent_ == null)
				cb_onKey_Landroid_view_View_ILandroid_view_KeyEvent_ = JNINativeWrapper.CreateDelegate (new _JniMarshal_PPLIL_Z (n_OnKey_Landroid_view_View_ILandroid_view_KeyEvent_));
			return cb_onKey_Landroid_view_View_ILandroid_view_KeyEvent_;
		}

		static bool n_OnKey_Landroid_view_View_ILandroid_view_KeyEvent_ (IntPtr jnienv, IntPtr native__this, IntPtr native_v, int native_keyCode, IntPtr native_e)
		{
			var __this = global::Java.Lang.Object.GetObject<global::Org.Libsdl.App.SDLSurface> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			var v = global::Java.Lang.Object.GetObject<global::Android.Views.View> (native_v, JniHandleOwnership.DoNotTransfer);
			var keyCode = (global::Android.Views.Keycode) native_keyCode;
			var e = global::Java.Lang.Object.GetObject<global::Android.Views.KeyEvent> (native_e, JniHandleOwnership.DoNotTransfer);
			bool __ret = __this.OnKey (v, keyCode, e);
			return __ret;
		}
#pragma warning restore 0169

		// Metadata.xml XPath method reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/method[@name='onKey' and count(parameter)=3 and parameter[1][@type='android.view.View'] and parameter[2][@type='int'] and parameter[3][@type='android.view.KeyEvent']]"
		[Register ("onKey", "(Landroid/view/View;ILandroid/view/KeyEvent;)Z", "GetOnKey_Landroid_view_View_ILandroid_view_KeyEvent_Handler")]
		public virtual unsafe bool OnKey (global::Android.Views.View v, [global::Android.Runtime.GeneratedEnum] global::Android.Views.Keycode keyCode, global::Android.Views.KeyEvent e)
		{
			const string __id = "onKey.(Landroid/view/View;ILandroid/view/KeyEvent;)Z";
			try {
				JniArgumentValue* __args = stackalloc JniArgumentValue [3];
				__args [0] = new JniArgumentValue ((v == null) ? IntPtr.Zero : ((global::Java.Lang.Object) v).Handle);
				__args [1] = new JniArgumentValue ((int) keyCode);
				__args [2] = new JniArgumentValue ((e == null) ? IntPtr.Zero : ((global::Java.Lang.Object) e).Handle);
				var __rm = _members.InstanceMethods.InvokeVirtualBooleanMethod (__id, this, __args);
				return __rm;
			} finally {
				global::System.GC.KeepAlive (v);
				global::System.GC.KeepAlive (e);
			}
		}

		static Delegate cb_onSensorChanged_Landroid_hardware_SensorEvent_;
#pragma warning disable 0169
		static Delegate GetOnSensorChanged_Landroid_hardware_SensorEvent_Handler ()
		{
			if (cb_onSensorChanged_Landroid_hardware_SensorEvent_ == null)
				cb_onSensorChanged_Landroid_hardware_SensorEvent_ = JNINativeWrapper.CreateDelegate (new _JniMarshal_PPL_V (n_OnSensorChanged_Landroid_hardware_SensorEvent_));
			return cb_onSensorChanged_Landroid_hardware_SensorEvent_;
		}

		static void n_OnSensorChanged_Landroid_hardware_SensorEvent_ (IntPtr jnienv, IntPtr native__this, IntPtr native_e)
		{
			var __this = global::Java.Lang.Object.GetObject<global::Org.Libsdl.App.SDLSurface> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			var e = global::Java.Lang.Object.GetObject<global::Android.Hardware.SensorEvent> (native_e, JniHandleOwnership.DoNotTransfer);
			__this.OnSensorChanged (e);
		}
#pragma warning restore 0169

		// Metadata.xml XPath method reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/method[@name='onSensorChanged' and count(parameter)=1 and parameter[1][@type='android.hardware.SensorEvent']]"
		[Register ("onSensorChanged", "(Landroid/hardware/SensorEvent;)V", "GetOnSensorChanged_Landroid_hardware_SensorEvent_Handler")]
		public virtual unsafe void OnSensorChanged (global::Android.Hardware.SensorEvent e)
		{
			const string __id = "onSensorChanged.(Landroid/hardware/SensorEvent;)V";
			try {
				JniArgumentValue* __args = stackalloc JniArgumentValue [1];
				__args [0] = new JniArgumentValue ((e == null) ? IntPtr.Zero : ((global::Java.Lang.Object) e).Handle);
				_members.InstanceMethods.InvokeVirtualVoidMethod (__id, this, __args);
			} finally {
				global::System.GC.KeepAlive (e);
			}
		}

		static Delegate cb_onTouch_Landroid_view_View_Landroid_view_MotionEvent_;
#pragma warning disable 0169
		static Delegate GetOnTouch_Landroid_view_View_Landroid_view_MotionEvent_Handler ()
		{
			if (cb_onTouch_Landroid_view_View_Landroid_view_MotionEvent_ == null)
				cb_onTouch_Landroid_view_View_Landroid_view_MotionEvent_ = JNINativeWrapper.CreateDelegate (new _JniMarshal_PPLL_Z (n_OnTouch_Landroid_view_View_Landroid_view_MotionEvent_));
			return cb_onTouch_Landroid_view_View_Landroid_view_MotionEvent_;
		}

		static bool n_OnTouch_Landroid_view_View_Landroid_view_MotionEvent_ (IntPtr jnienv, IntPtr native__this, IntPtr native_v, IntPtr native_e)
		{
			var __this = global::Java.Lang.Object.GetObject<global::Org.Libsdl.App.SDLSurface> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			var v = global::Java.Lang.Object.GetObject<global::Android.Views.View> (native_v, JniHandleOwnership.DoNotTransfer);
			var e = global::Java.Lang.Object.GetObject<global::Android.Views.MotionEvent> (native_e, JniHandleOwnership.DoNotTransfer);
			bool __ret = __this.OnTouch (v, e);
			return __ret;
		}
#pragma warning restore 0169

		// Metadata.xml XPath method reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/method[@name='onTouch' and count(parameter)=2 and parameter[1][@type='android.view.View'] and parameter[2][@type='android.view.MotionEvent']]"
		[Register ("onTouch", "(Landroid/view/View;Landroid/view/MotionEvent;)Z", "GetOnTouch_Landroid_view_View_Landroid_view_MotionEvent_Handler")]
		public virtual unsafe bool OnTouch (global::Android.Views.View v, global::Android.Views.MotionEvent e)
		{
			const string __id = "onTouch.(Landroid/view/View;Landroid/view/MotionEvent;)Z";
			try {
				JniArgumentValue* __args = stackalloc JniArgumentValue [2];
				__args [0] = new JniArgumentValue ((v == null) ? IntPtr.Zero : ((global::Java.Lang.Object) v).Handle);
				__args [1] = new JniArgumentValue ((e == null) ? IntPtr.Zero : ((global::Java.Lang.Object) e).Handle);
				var __rm = _members.InstanceMethods.InvokeVirtualBooleanMethod (__id, this, __args);
				return __rm;
			} finally {
				global::System.GC.KeepAlive (v);
				global::System.GC.KeepAlive (e);
			}
		}

		static Delegate cb_surfaceChanged_Landroid_view_SurfaceHolder_III;
#pragma warning disable 0169
		static Delegate GetSurfaceChanged_Landroid_view_SurfaceHolder_IIIHandler ()
		{
			if (cb_surfaceChanged_Landroid_view_SurfaceHolder_III == null)
				cb_surfaceChanged_Landroid_view_SurfaceHolder_III = JNINativeWrapper.CreateDelegate (new _JniMarshal_PPLIII_V (n_SurfaceChanged_Landroid_view_SurfaceHolder_III));
			return cb_surfaceChanged_Landroid_view_SurfaceHolder_III;
		}

		static void n_SurfaceChanged_Landroid_view_SurfaceHolder_III (IntPtr jnienv, IntPtr native__this, IntPtr native_holder, int native_format, int width, int height)
		{
			var __this = global::Java.Lang.Object.GetObject<global::Org.Libsdl.App.SDLSurface> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			var holder = (global::Android.Views.ISurfaceHolder)global::Java.Lang.Object.GetObject<global::Android.Views.ISurfaceHolder> (native_holder, JniHandleOwnership.DoNotTransfer);
			var format = (global::Android.Graphics.Format) native_format;
			__this.SurfaceChanged (holder, format, width, height);
		}
#pragma warning restore 0169

		// Metadata.xml XPath method reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/method[@name='surfaceChanged' and count(parameter)=4 and parameter[1][@type='android.view.SurfaceHolder'] and parameter[2][@type='int'] and parameter[3][@type='int'] and parameter[4][@type='int']]"
		[Register ("surfaceChanged", "(Landroid/view/SurfaceHolder;III)V", "GetSurfaceChanged_Landroid_view_SurfaceHolder_IIIHandler")]
		public virtual unsafe void SurfaceChanged (global::Android.Views.ISurfaceHolder holder, [global::Android.Runtime.GeneratedEnum] global::Android.Graphics.Format format, int width, int height)
		{
			const string __id = "surfaceChanged.(Landroid/view/SurfaceHolder;III)V";
			try {
				JniArgumentValue* __args = stackalloc JniArgumentValue [4];
				__args [0] = new JniArgumentValue ((holder == null) ? IntPtr.Zero : ((global::Java.Lang.Object) holder).Handle);
				__args [1] = new JniArgumentValue ((int) format);
				__args [2] = new JniArgumentValue (width);
				__args [3] = new JniArgumentValue (height);
				_members.InstanceMethods.InvokeVirtualVoidMethod (__id, this, __args);
			} finally {
				global::System.GC.KeepAlive (holder);
			}
		}

		static Delegate cb_surfaceCreated_Landroid_view_SurfaceHolder_;
#pragma warning disable 0169
		static Delegate GetSurfaceCreated_Landroid_view_SurfaceHolder_Handler ()
		{
			if (cb_surfaceCreated_Landroid_view_SurfaceHolder_ == null)
				cb_surfaceCreated_Landroid_view_SurfaceHolder_ = JNINativeWrapper.CreateDelegate (new _JniMarshal_PPL_V (n_SurfaceCreated_Landroid_view_SurfaceHolder_));
			return cb_surfaceCreated_Landroid_view_SurfaceHolder_;
		}

		static void n_SurfaceCreated_Landroid_view_SurfaceHolder_ (IntPtr jnienv, IntPtr native__this, IntPtr native_holder)
		{
			var __this = global::Java.Lang.Object.GetObject<global::Org.Libsdl.App.SDLSurface> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			var holder = (global::Android.Views.ISurfaceHolder)global::Java.Lang.Object.GetObject<global::Android.Views.ISurfaceHolder> (native_holder, JniHandleOwnership.DoNotTransfer);
			__this.SurfaceCreated (holder);
		}
#pragma warning restore 0169

		// Metadata.xml XPath method reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/method[@name='surfaceCreated' and count(parameter)=1 and parameter[1][@type='android.view.SurfaceHolder']]"
		[Register ("surfaceCreated", "(Landroid/view/SurfaceHolder;)V", "GetSurfaceCreated_Landroid_view_SurfaceHolder_Handler")]
		public virtual unsafe void SurfaceCreated (global::Android.Views.ISurfaceHolder holder)
		{
			const string __id = "surfaceCreated.(Landroid/view/SurfaceHolder;)V";
			try {
				JniArgumentValue* __args = stackalloc JniArgumentValue [1];
				__args [0] = new JniArgumentValue ((holder == null) ? IntPtr.Zero : ((global::Java.Lang.Object) holder).Handle);
				_members.InstanceMethods.InvokeVirtualVoidMethod (__id, this, __args);
			} finally {
				global::System.GC.KeepAlive (holder);
			}
		}

		static Delegate cb_surfaceDestroyed_Landroid_view_SurfaceHolder_;
#pragma warning disable 0169
		static Delegate GetSurfaceDestroyed_Landroid_view_SurfaceHolder_Handler ()
		{
			if (cb_surfaceDestroyed_Landroid_view_SurfaceHolder_ == null)
				cb_surfaceDestroyed_Landroid_view_SurfaceHolder_ = JNINativeWrapper.CreateDelegate (new _JniMarshal_PPL_V (n_SurfaceDestroyed_Landroid_view_SurfaceHolder_));
			return cb_surfaceDestroyed_Landroid_view_SurfaceHolder_;
		}

		static void n_SurfaceDestroyed_Landroid_view_SurfaceHolder_ (IntPtr jnienv, IntPtr native__this, IntPtr native_holder)
		{
			var __this = global::Java.Lang.Object.GetObject<global::Org.Libsdl.App.SDLSurface> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			var holder = (global::Android.Views.ISurfaceHolder)global::Java.Lang.Object.GetObject<global::Android.Views.ISurfaceHolder> (native_holder, JniHandleOwnership.DoNotTransfer);
			__this.SurfaceDestroyed (holder);
		}
#pragma warning restore 0169

		// Metadata.xml XPath method reference: path="/api/package[@name='org.libsdl.app']/class[@name='SDLSurface']/method[@name='surfaceDestroyed' and count(parameter)=1 and parameter[1][@type='android.view.SurfaceHolder']]"
		[Register ("surfaceDestroyed", "(Landroid/view/SurfaceHolder;)V", "GetSurfaceDestroyed_Landroid_view_SurfaceHolder_Handler")]
		public virtual unsafe void SurfaceDestroyed (global::Android.Views.ISurfaceHolder holder)
		{
			const string __id = "surfaceDestroyed.(Landroid/view/SurfaceHolder;)V";
			try {
				JniArgumentValue* __args = stackalloc JniArgumentValue [1];
				__args [0] = new JniArgumentValue ((holder == null) ? IntPtr.Zero : ((global::Java.Lang.Object) holder).Handle);
				_members.InstanceMethods.InvokeVirtualVoidMethod (__id, this, __args);
			} finally {
				global::System.GC.KeepAlive (holder);
			}
		}

	}
}
