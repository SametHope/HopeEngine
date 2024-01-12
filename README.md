# HopeEngine (or more like Particle System 'Game')
HopeEngine is a super basic 'game engine' that is technically just a basic wrapper around [RayLib](https://www.raylib.com/), specifically [CsLo bindings](https://github.com/NotNotTech/Raylib-CsLo) of it. 
It doesn't even have any proper games. It only has the Particle Game which I think is still pretty neat.  
But this is fine as this project, unlike its name may imply, doesn't intend to be a game engine of any sorts. It is just me trying out, (re)inventing and implementing sorts of things without any game engine involved from scratch as a learning practice and for fun.

# Particle Game
This is a 'game' that basically simulates a lot of particles simply with their velocities, using mouse input to dedice whether attract or repel and using mouse position to scale the force of the action for each particle.  
There are 256 000 particles being simulated.
Update and draw logic are decoupled so drawing can be turned off, or the amount that is draws can be modified.
You can reset the positions and velocities of the particles.
You can sort-of rescale and reset the particles according to that.

All the features of the game are really written on it as green texts on top left anyway. Here are some cool pictures from the 'game'.
![2024-01-12 23_40_36-The 'ParticleGame' 1 1 5](https://github.com/SametHope/HopeEngine/assets/85421686/de11c82b-d7e5-4a22-82e3-bc331294f4f2)
![2024-01-12 23_39_44-The 'ParticleGame' 1 1 5](https://github.com/SametHope/HopeEngine/assets/85421686/fde01f0b-2994-4cce-8dc8-c9c9d70e1c07)
![2024-01-12 23_37_01-The 'ParticleGame' 1 1 5](https://github.com/SametHope/HopeEngine/assets/85421686/8a08a3c3-a04c-4e12-ba3f-82471a0181ca)
![2024-01-12 23_36_42-The 'ParticleGame' 1 1 5](https://github.com/SametHope/HopeEngine/assets/85421686/ea0cebbe-3dea-4038-a449-665d64f5f7c8)
![2024-01-12 23_36_10-The 'ParticleGame' 1 1 5](https://github.com/SametHope/HopeEngine/assets/85421686/8d91969d-74ce-4cef-9edf-1f6f0d0bba2e)
![2024-01-12 23_35_24-The 'ParticleGame' 1 1 5](https://github.com/SametHope/HopeEngine/assets/85421686/2adf6ccf-02f6-43ed-a10b-1658c2a295aa)

## After-Thoughs
I could probably spend more time on figure out a better way to render the particles which is what most performance-heavy this is currently. Probably having the particles drawn on a texture and then drawing that texture could have been better.  

For even better performance, using seperate threads for render and update logic would be great, however RayLib uses a OpenGL context which can not be written from another thread than it was created from as far as I know.
There probably are some workarunds for this but I don't feel like tackling this problem.  

I also could make a proper GUI where we could select colors, repelsion and attraction powers and make the particle count actually change rather than just not drawing them and acting as if they are gone.  

Making another basic project within the project using the 'engine' (or looper as I call it often) would be pretty fun too, but after working on this project I learned to love game engines more for many reasons lol.
 
