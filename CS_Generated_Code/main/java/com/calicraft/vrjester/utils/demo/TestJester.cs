using System.Collections.Generic;

namespace com.calicraft.vrjester.utils.demo
{
	using Config = com.calicraft.vrjester.config.Config;
	using Constants = com.calicraft.vrjester.config.Constants;
	using VRDataState = com.calicraft.vrjester.utils.vrdata.VRDataState;
	using ParticleTypes = net.minecraft.core.particles.ParticleTypes;
	using SimpleParticleType = net.minecraft.core.particles.SimpleParticleType;
	using Vec3 = net.minecraft.world.phys.Vec3;

	using static com.calicraft.vrjester.utils.demo.SpawnParticles.moveParticles;

	public class TestJester
	{

		private static readonly SimpleParticleType[] particleTypes = new SimpleParticleType[]{ParticleTypes.FLAME, ParticleTypes.SOUL_FIRE_FLAME, ParticleTypes.DRAGON_BREATH, ParticleTypes.CLOUD, ParticleTypes.BUBBLE_POP, ParticleTypes.FALLING_WATER};

		public TestJester()
		{
		}

		public virtual void trigger(Dictionary<string, string> gesture, VRDataState vrDataWorldPre, Config config)
		{
			Config.ParticleContext gestureCtx = config.TESTING_GESTURES[gesture["gestureName"]];
			if (gestureCtx == null)
			{
				gestureCtx = new Config.ParticleContext(config, 1.0, 0, 0);
			}

			Vec3 avgDir = vrDataWorldPre.Rc[1].add(vrDataWorldPre.Hmd[1]).multiply((.5), (.5), (.5));
			if (gestureCtx.rcParticle > -1 && gestureCtx.rcParticle < particleTypes.Length && gesture.ContainsKey(Constants.RC))
			{
				moveParticles(particleTypes[gestureCtx.rcParticle], vrDataWorldPre.Rc[0], avgDir, gestureCtx.velocity);
			}
			if (gestureCtx.lcParticle > -1 && gestureCtx.lcParticle < particleTypes.Length && gesture.ContainsKey(Constants.LC))
			{
				if (!gesture.ContainsKey(Constants.RC))
				{
					avgDir = vrDataWorldPre.Lc[1].add(vrDataWorldPre.Hmd[1]).multiply((.5), (.5), (.5));
				}
				moveParticles(particleTypes[gestureCtx.lcParticle], vrDataWorldPre.Lc[0], avgDir, gestureCtx.velocity);
			}
		}
	}

}
